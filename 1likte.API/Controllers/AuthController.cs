using System;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace _1likte.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        // User Login
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Kullanıcı Girişi",
            Description = "Bu işlem, kullanıcıyı sisteme giriş yapmasını sağlar. Başarılı girişte, erişim token'ı döner.",
            OperationId = "Login",
            Tags = new[] { "Kimlik Doğrulama" }
        )]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestModel loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var response = await _authService.AuthenticateAsync(loginDto);
                HttpContext.Response.Cookies.Append("access-token", response.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = response.AccessTokenExpiration
                });

                HttpContext.Response.Headers["X-Access-Token"] = response.AccessToken;

                return Ok(new
                {
                    Message = "Giriş başarılı!",
                    UserId = response.UserId,
                    FullName = response.FullName,
                    ProfilePhotoUrl = response.ProfilePhotoUrl,
                    Email = response.Email,
                });
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

        // User Registration
        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Kullanıcı Kayıt",
            Description = "Bu işlem, kullanıcı kaydını gerçekleştirir. Başarılı kayıtta, erişim token'ı döner.",
            OperationId = "Register",
            Tags = new[] { "Kimlik Doğrulama" }
        )]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestModel createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var response = await _authService.RegisterAsync(createUserDto);
                HttpContext.Response.Cookies.Append("access-token", response.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = response.AccessTokenExpiration
                });

                HttpContext.Response.Headers["X-Access-Token"] = response.AccessToken;

                return Ok(new
                {
                    Message = "Kayıt Başarılı!",
                    UserId = response.UserId,
                    FullName = response.FullName,
                    ProfilePhotoUrl = response.ProfilePhotoUrl,
                    Email = response.Email,
                });
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

        // Refresh Token
        [HttpPost("refresh")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerOperation(
            Summary = "Erişim Token'ını Yenile",
            Description = "Bu işlem, geçerli refresh token ile yeni bir erişim token'ı oluşturur.",
            OperationId = "RefreshToken",
            Tags = new[] { "Kimlik Doğrulama" }
        )]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest(new { message = "Token eksik." });

            try
            {
                var newToken = await _authService.RefreshTokenAsync(refreshToken);
                return Ok(newToken);
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

        // Revoke Token
        [HttpPost("revoke")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerOperation(
            Summary = "Erişim Token'ını İptal Et",
            Description = "Bu işlem, verilen refresh token'ı iptal eder.",
            OperationId = "RevokeToken",
            Tags = new[] { "Kimlik Doğrulama" }
        )]
        public async Task<IActionResult> RevokeToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest(new { message = "Token eksik." });
            try
            {
                var isRevoked = await _authService.RevokeRefreshTokenAsync(refreshToken);
                if (!isRevoked)
                    return BadRequest(new { message = "Token geçersiz." });

                return Ok(new { message = "Token başarıyla iptal edildi." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Bir hata oluştu", details = ex.Message });
            }
        }
    }
}
