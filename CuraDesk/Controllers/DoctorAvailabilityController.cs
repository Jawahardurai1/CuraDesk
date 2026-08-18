using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CuraDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorAvailabilityController:ControllerBase
    {
        private readonly IDoctorAvailabilityService _service;
        private Guid CurrentUserId =>
          Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public DoctorAvailabilityController(IDoctorAvailabilityService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("availability")]

        public async Task<IActionResult> AddAvailability(CreateDoctorAvailabilityDto dto)
        {
            var res = await _service.AddAvailabilityAsync(CurrentUserId,dto);
            if(res==null) { return BadRequest(new {message ="Invalid time Range"}); }

            return StatusCode(201,res);
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("{doctorId}/availability")]
        public async Task<IActionResult> GetDoctorAvailability(Guid doctorId)
        {
            var result = await _service.GetDoctorAvailabilityAsync(doctorId);
            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("availability/me")]
        public async Task<IActionResult> GetMyAvailability()
        {
            var result = await _service.GetDoctorAvailabilityAsync(CurrentUserId);
            return Ok(result);
        }
    }
}
