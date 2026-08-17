using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace CuraDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var res = await _authService.LoginAsync(dto);
            if(res==null) { return Unauthorized(new { message = "Invalid Email Id or Password" });
            }
            return Ok(res);
        }

    }
}
