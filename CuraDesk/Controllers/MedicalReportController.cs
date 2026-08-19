using CuraDesk.Business.Interface.Service;
using CuraDesk.Business.Services;
using CuraDesk.Model.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CuraDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalReportController:ControllerBase
    {
        private readonly IMedicalReportService _reportService;
        private readonly ICloudinaryService _cloudinaryService;
        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private string CurrentRole => User.FindFirstValue(ClaimTypes.Role)!;
        public MedicalReportController(IMedicalReportService reportService, ICloudinaryService cloudinaryService)
        {
            _reportService = reportService;
            _cloudinaryService = cloudinaryService;
        }
        [Authorize (Roles ="Patient")]
        [HttpPost]
        public async Task<IActionResult> Upload( [FromForm] UploadReportMetadataDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest(new { message = "No file uploaded" });

            if (dto.File.ContentType != "application/pdf")
                return BadRequest(new { message = "Only PDF files are allowed" });

            if (dto.File.Length > 5 * 1024 * 1024)
                return BadRequest(new { message = "File size exceeds 5MB limit" });

            using var stream = dto.File.OpenReadStream();
            var fileUrl = await _cloudinaryService.UploadPdfAsync(stream, dto.File.FileName);

            var result = await _reportService.UploadReportAsync(CurrentUserId, dto.File.FileName, fileUrl, dto);
            return StatusCode(201, result);
        }
        [Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> GetMyReports([FromQuery] DateTime? date, [FromQuery] string? diagnosisTag)
        {
            var result = await _reportService.GetMyReportsAsync(CurrentUserId, date, diagnosisTag);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{id}/view")]
        public async Task<IActionResult> Download(Guid id)
        {
            var report = await _reportService.GetReportForAccessCheckAsync(CurrentUserId, CurrentRole, id);
            if (report == null)
                return Forbid();

            return Ok(new { fileName = report.FileName, downloadUrl = report.FilePath });
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("patient/{patientUserId}")]
        public async Task<IActionResult> GetPatientReports(Guid patientUserId)
        {
            var result = await _reportService.GetPatientReportsForDoctorAsync(CurrentUserId, patientUserId);
            if (result == null)
                return Forbid();

            return Ok(result);
        }


    }
}
