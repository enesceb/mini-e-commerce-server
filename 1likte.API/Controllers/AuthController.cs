using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("login")]
        [AllowAnonymous]
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

        [HttpPost("register")]
        [AllowAnonymous]

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

                // Header'a token ekle
                HttpContext.Response.Headers["X-Access-Token"] = response.AccessToken;

                // Başarılı yanıt dön
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

        [HttpPost("refresh")]
       [Authorize(Roles ="Admin , User")] 
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


        [HttpPost("revoke")]
       [Authorize(Roles ="Admin , User")] 
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