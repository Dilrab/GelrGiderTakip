using ApiGelirGider.WebApi.Context;
using IncomeExpenseTracker.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGelirGider.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 🔐 Kullanıcı girişi ve token üretimi
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            var user = _context.Users
                .FirstOrDefault(x =>
                    x.UserEmail == loginDto.UserEmail &&
                    x.Password == loginDto.Password); // Şifre hash'li değilse dikkat!

            if (user is null)
                return Unauthorized(new { message = "Geçersiz kullanıcı bilgileri" });

            var (token, expiresAt) = GenerateJwtToken(user);

            return Ok(new LoginResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.UserEmail,
                    FullName = user.UserName,
                    Role = "User"
                }
            });
        }

        // 🔐 JWT Token üretimi
        private (string token, DateTime ExpiresAt) GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // → "nameid"
                new Claim("id", user.Id.ToString()),                      // → özel claim
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "User")                        // → rol tanımı
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);

            var jwt = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expires);
        }
    }

    // ✅ DTO'lar
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new();
        public DateTime ExpiresAt { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }

    public class UserLoginDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
