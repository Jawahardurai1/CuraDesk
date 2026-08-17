using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CuraDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController:ControllerBase
    {
        private readonly IDoctorProfile doctorProfile;
        private Guid CurrentUserId=>Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public DoctorController(IDoctorProfile _doctorProfile)
        {
            doctorProfile= _doctorProfile;
        }
        [Authorize(Roles ="Admin,Doctor")]
        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateDoctorProfileDto dto)
        {
            Console.WriteLine("Controller reaxhecx");
            var result = await doctorProfile.CreateProfileAsync(CurrentUserId, dto);
            if (result == null)
                return BadRequest(new { message = "Profile already exists or invalid user" });

            return StatusCode(201, result);
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("profile/me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await doctorProfile.GetProfileByUserIdAsync(CurrentUserId);
            if (result == null)
                return NotFound(new { message = "Profile not found" });

            return Ok(result);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPut("profile/me")]
        public async Task<IActionResult> UpdateMyProfile(UpdateDoctorProfileDto dto)
        {
            var result = await doctorProfile.UpdateProfileAsync(CurrentUserId, dto);
            if (result == null)
                return NotFound(new { message = "Profile not found" });

            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllDoc()
        {
            var res = await doctorProfile.GetAllDoctorsAsync();
            return Ok(res);
        }
    }
}
