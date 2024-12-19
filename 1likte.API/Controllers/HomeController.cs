using Microsoft.AspNetCore.Mvc;

namespace _1likte.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome to 1likte API!");
        }
    }
}
