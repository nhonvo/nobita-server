using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HDBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok();
        }
        [HttpPost("register")]
        public IActionResult Register()
        {
            return Ok();
        }
        [HttpPost("change-password")]
        public IActionResult ChangePassword()
        {
            return Ok();
        }
        [HttpGet("balance")]
        public IActionResult Balance()
        {
            return Ok();
        }
    }
}
