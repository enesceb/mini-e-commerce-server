using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.API.Configurations;
using _1likte.Core.Services;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _1likte.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AuthService _authService;

        public UserController(IUserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Kullanıcı doğrulama işlemi (daha önceki gibi)
            var user = _userService.Login(request);
            if (user == null)
                return Unauthorized("Geçersiz kullanıcı veya şifre");

            // JWT oluştur ve cookie'ye ekle
            var result = _authService.WriteToken(user);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserDetail(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var createdUser = await _userService.RegisterUser(user);
            return Ok(createdUser);
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            var createdUser = await _userService.UpdateUser(user);
            return Ok(createdUser);
        }
    }
}