
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sampleAppilcation.Models;

namespace BasicAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
 
    public class WelcomeController : ControllerBase
    {
        [HttpPost]
        public IActionResult Welcome(WelcomeRequest request)
        {
            var response = new WelcomeResponse
            {
                Username = request.UserName,

                WelcomeMessage =
                    $"{request.WelcomeMessage} Mr. {request.UserName}",

                Role = "General User"
            };

            return Ok(response);
        }
    }
}