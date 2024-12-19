using _1likte.Core.Services;
using _1likte.Data;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _1likte.Core.Concrete
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthenticationService(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<TokenModel> AuthenticateAsync(UserLoginRequestModel loginDto)
        {
            try
            {
                // Kullanıcıyı kontrol et
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user == null)
                {
                    throw new UnauthorizedAccessException("Kullanıcı adı veya şifre hatalı!");
                }
                var passwordHasher = new PasswordHasher<User>();
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    throw new UnauthorizedAccessException("Kullanıcı adı veya şifre hatalı!");
                }

                var isHaveToken = await _context.Tokens.FirstOrDefaultAsync(t => t.UserId == user.Id && t.IsDeleted == false);
                if (isHaveToken != null)
                {
                    isHaveToken.IsDeleted = true;
                    await _context.SaveChangesAsync();
                }

                // Token üret
                var token = _tokenService.GenerateToken(user);

                // Token'ı veritabanına kaydet
                await _context.Tokens.AddAsync(new Token
                {
                    UserId = user.Id,
                    AccessToken = token.AccessToken,
                    AccessTokenExpiration = token.AccessTokenExpiration,
                    RefreshToken = token.RefreshToken,
                    RefreshTokenExpiration = token.RefreshTokenExpiration
                });

                await _context.SaveChangesAsync();

                return token;
            }
            catch (UnauthorizedAccessException ex)
            {
                // Unauthorized hatası için özelleştirilmiş mesaj
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                // Diğer hatalar için loglama ve hata fırlatma
                throw new Exception("Bir hata oluştu: " + ex.Message);
            }
        }

        public async Task<TokenModel> RegisterAsync(UserRegisterRequestModel registerDto)
        {
            try
            {
                var isUserExist = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);
                if (isUserExist)
                {
                    throw new UnauthorizedAccessException("Bu e-posta adresi kayıt olmak için kullanılamaz!");
                }
                var passwordHasher = new PasswordHasher<User>();

                // Kullanıcıyı veritabanına kaydet
                var user = new User
                {
                    Email = registerDto.Email,
                    PasswordHash = passwordHasher.HashPassword(null, registerDto.Password),
                    FullName = registerDto.FirstName,
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                // Token üret
                var token = _tokenService.GenerateToken(user);
                // Token'ı veritabanına kaydet
                await _context.Tokens.AddAsync(new Token
                {
                    UserId = user.Id,
                    AccessToken = token.AccessToken,
                    AccessTokenExpiration = token.AccessTokenExpiration,
                    RefreshToken = token.RefreshToken,
                    RefreshTokenExpiration = token.RefreshTokenExpiration
                });
                await _context.SaveChangesAsync();

                return token;
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama
                throw new Exception("Bir hata oluştu: " + ex.Message);
            }
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            try
            {
                // Refresh token'ın doğruluğunu kontrol et
                var token = await _context.Tokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken && t.IsDeleted == false);
                if (token == null) return false;

                // Refresh token'ı iptal et
                token.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama
                throw new Exception("Bir hata oluştu: " + ex.Message);
            }
        }

        public async Task<TokenModel> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                // Refresh token doğruluğunu kontrol et
                var token = await _context.Tokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken && t.IsDeleted == false);
                if (token == null || token.RefreshTokenExpiration <= DateTime.Now)
                {
                    throw new UnauthorizedAccessException("Geçersiz veya süresi dolmuş refresh token!");
                }

                // Kullanıcıyı al
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == token.UserId);
                if (user == null) throw new UnauthorizedAccessException("Kullanıcı bulunamadı!");

                // Yeni token üret
                var newToken = _tokenService.GenerateToken(user);

                // Eski refresh token'ı güncelle
                token.RefreshToken = newToken.RefreshToken;
                token.RefreshTokenExpiration = newToken.RefreshTokenExpiration;
                token.AccessToken = newToken.AccessToken;
                token.AccessTokenExpiration = newToken.AccessTokenExpiration;
                token.RefreshedAt = DateTime.Now;

                _context.Tokens.Update(token);
                await _context.SaveChangesAsync();

                return newToken;
            }
            catch (UnauthorizedAccessException ex)
            {
                // Hata durumunda UnauthorizedAccessException fırlatıyoruz
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                // Diğer hata durumları için genel exception
                throw new Exception("Bir hata oluştu: " + ex.Message);
            }
        }
    }
}