using System.Security.Claims;
using System.Text;
using App.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace App.Services;

public class JwtTokenProvider(IConfiguration configuration)
{
    public string GenerateJwtToken(User user)
    {
        string secret_key = configuration["Jwt:Secret"]!;
        var security_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key));

        var credentials = new SigningCredentials(security_key, SecurityAlgorithms.HmacSha256);

        var token_descriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.login),
                new Claim(JwtRegisteredClaimNames.Email, user.email ?? "urmom@my.bed"),
                new Claim("user_login", user.login),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("jwt:expiration_minutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(token_descriptor);
    }

    public void AppendTokenCookie(IResponseCookies cookies, User user)
    {
        var token = GenerateJwtToken(user);

        var options = new CookieOptions{
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1),
        };
        cookies.Append("jwt_token", token, options);
    }
}
