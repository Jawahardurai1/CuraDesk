using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CuraDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaitentProfileController:ControllerBase
    {
        private readonly IPatientProfileService _profileService;
        private Guid CurrentUserId =>
          Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public PaitentProfileController(IPatientProfileService patientProfileService)
        {
            _profileService = patientProfileService;
        }

        [Authorize(Roles ="Patient")]
        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreatePatientProfileDto dto)
        {
            var res = await _profileService.CreateProfileAsync(CurrentUserId, dto);
            if(res==null)
            {
                return BadRequest(new { message = "Profile already exists or invalid user" });
            }
            return StatusCode(201, res);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("me")]
        public async Task<IActionResult> GetmyProfile()
        {
            var result = await _profileService.GetProfileByUserIdAsync(CurrentUserId);
            if (result == null)
                return NotFound(new { message = "Profile not found" });

            return Ok(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(UpdatePatientProfileDto dto)
        {
            var result = await _profileService.UpdateProfileAsync(CurrentUserId, dto);
            if (result == null)
                return NotFound(new { message = "Profile not found" });

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("{patientUserId}")]
        public async Task<IActionResult> GetPatientProfileForDoctor(Guid patientUserId)
        {
            var result = await _profileService.GetProfileForDoctorAsync(CurrentUserId, patientUserId);
            if (result == null)
                return Forbid();  

            return Ok(result);
        }

    }
}
