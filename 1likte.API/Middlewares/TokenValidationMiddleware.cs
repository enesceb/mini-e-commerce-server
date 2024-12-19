using System.IdentityModel.Tokens.Jwt;

namespace TaskManagementSystemBackend.API.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenValidationMiddleware> _logger;

        public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>() == null)
            {
                await _next(context);
                return;
            }

            // Token kontrolü
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var jwtToken = handler.ReadJwtToken(token);

                        var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                        if (expClaim != null)
                        {
                            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim));
                            if (expirationDate <= DateTimeOffset.UtcNow)
                            {
                                _logger.LogWarning("Token süresi dolmuş.");
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                await context.Response.WriteAsJsonAsync(new { message = "Token süresi dolmuş." });
                                return;
                            }
                        }
                        var userId = int.Parse(jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value);
                        context.Items["UserId"] = userId;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Token doğrulama sırasında hata oluştu.");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Geçersiz token." });
                    return;
                }
            }

            await _next(context);
        }
    }
}
