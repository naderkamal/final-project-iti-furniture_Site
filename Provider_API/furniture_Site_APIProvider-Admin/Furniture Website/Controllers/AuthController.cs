using Furniture_Website.DTO;
using Furniture_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Furniture_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AdminDashboardMVC_DBContext _context;

        public AuthController(IConfiguration config, AdminDashboardMVC_DBContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            AspNetUser userlogin = new AspNetUser()
            {
                Email = loginDTO.Email,
                Password = loginDTO.Password,
            };
            var user = await _context.AspNetUsers.SingleOrDefaultAsync(u => u.Email == userlogin.Email && u.Password == userlogin.Password);
            if (user == null)
                return Unauthorized();

            var token = GenerateJwtToken(user);

            return Ok(new { token, user.Id, user.Email });
        }

        private string GenerateJwtToken(AspNetUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
