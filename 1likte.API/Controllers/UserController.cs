using _1likte.Core.Services;
using _1likte.Model.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        // Get All Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Tüm kullanıcıları getirir",
            Description = "Bu işlem, tüm kullanıcıların listesini getirir. Eğer bir hata oluşursa, hata mesajı ile birlikte bir 500 Internal Server Error döner.",
            OperationId = "GetAllUsers",
            Tags = new[] { "Kullanıcı Yönetimi" }
        )]
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

        // Get User Details by Id
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerOperation(
            Summary = "Kullanıcı detaylarını getirir",
            Description = "Bu işlem, verilen kullanıcı ID'sine göre kullanıcı detaylarını getirir. Eğer kullanıcı bulunamazsa, 404 Not Found hatası döner.",
            OperationId = "GetUserDetail",
            Tags = new[] { "Kullanıcı Yönetimi" }
        )]
        public async Task<IActionResult> GetUserDetail(int id)
        {
            try
            {
                if (id == 0 || id < 0) return BadRequest(); 
                var user = await _userService.GetUserById(id);
                if (user.Data == null) return NotFound();
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Bir hata oluştu", details = ex.Message });
            }
        }

        // Update User
        [HttpPost("update")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerOperation(
            Summary = "Kullanıcıyı günceller",
            Description = "Bu işlem, kullanıcı bilgilerini günceller. Kullanıcı doğrulaması gereklidir. Eğer bir hata oluşursa, hata mesajı ile birlikte 404 Not Found hatası döner.",
            OperationId = "UpdateUser",
            Tags = new[] { "Kullanıcı Yönetimi" }
        )]
        public async Task<IActionResult> Update([FromBody] UserUpdateModel user)
        {
            try
            {
                if (user == null) return BadRequest();
                var updatedUser = await _userService.UpdateUser(user);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Bir hata oluştu", details = ex.Message });
            }
        }
    }
}
