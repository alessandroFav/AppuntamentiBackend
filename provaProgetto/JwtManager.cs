using provaProgetto.Controllers;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace provaProgetto
{
    public class JwtManager
    {
        private readonly IConfiguration _configuration;
        public JwtManager(IConfiguration config)
        {
            _configuration = config;
        }
        public string GenerateJwtToken(Utente user)
        {
            var securityKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.password)
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
