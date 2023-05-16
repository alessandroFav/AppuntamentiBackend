using provaProgetto.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MimeKit.Cryptography;
using provaProgetto.Dipendenze;

namespace provaProgetto
{
    public class JwtManager: IJwtManager
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
                new Claim("id", user.id.ToString()),
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public int? ValidateToken(string token)
        {
            if(token == null)
                return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuers = new string[] { _configuration["Jwt:Issuer"] },
                    ValidAudiences = new string[] { _configuration["Jwt:Issuer"] },
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}
