using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CuraDesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [Authorize(Roles ="Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(CreateUserDto Dto)
        {
            var result = await _userService.AddUserAsync(Dto);
            if (result == null)
            {
                return BadRequest(new { message = "Email already Exists" });
            }
            return StatusCode(201, result);

        }


    }
}
