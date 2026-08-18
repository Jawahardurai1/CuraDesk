using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CuraDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController:ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private Guid CurrentUserId=> Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [Authorize(Roles = "Patient")]
        [HttpPost("book")]
        public async Task<IActionResult> Book(BookAppointmentDto dto)
        {
            var result = await _appointmentService.BookAppointmentAsync(CurrentUserId, dto);
            if (result == null)
                return Conflict(new { message = "Slot unavailable, or complete your patient profile before booking" });

            return StatusCode(201, result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var result = await _appointmentService.GetMyAppointmentsAsPatientAsync(CurrentUserId);
            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            var result = await _appointmentService.GetMyAppointmentsAsDoctorAsync(CurrentUserId);
            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("accept/{appointmentId}")]
        public async Task<IActionResult> UpdateAccepted(Guid appointmentId)
        {
            var result = await _appointmentService.UpdateAcceptance(appointmentId);
            

            if (result == null)
            {
                return BadRequest(new
                {
                    message = "Appointment could not be accepted"
                });
            }

            return Ok(result);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPut("Reject/{appointmentId}")]
        public async Task<IActionResult> UpdateRejected(Guid appointmentId)
        {
            var result = await _appointmentService.UpdateRejected(appointmentId);


            if (result != null)
            {
                return BadRequest(new
                {
                    message = "Appointment could not be accepted"
                });
            }

            return Ok(result);
        }

    }
}
