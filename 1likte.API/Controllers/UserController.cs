using _1likte.Core.Services;
using _1likte.Model.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _1likte.API.Controllers
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

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserDetail(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Bir hata oluştu", details = ex.Message });
            }
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserUpdateModel user)
        {
            var createdUser = await _userService.UpdateUser(user);
            return Ok(createdUser);
        }

    }
}