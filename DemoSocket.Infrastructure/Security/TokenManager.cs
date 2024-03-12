using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoSocket.Infrastructure.Security
{
    public class TokenManager(JwtSecurityTokenHandler _handler, TokenManager.Config _config)
    {
        public class Config
        {
            public string Signature { get; set; } = null!;
            public string? Issuer { get; set; }
            public string? Audience { get; set; }
            public int? Duration { get; set; }
        }

        private SecurityKey SecurityKey {
            get => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Signature));
        }

        public string CreateToken(string name)
        {
            JwtSecurityToken token = new JwtSecurityToken(
                _config.Issuer,
                _config.Audience,
                [new Claim(ClaimTypes.NameIdentifier, name)],
                DateTime.Now,
                DateTime.Now.AddSeconds(_config.Duration ?? 86400),
                new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256)
            );
            return _handler.WriteToken(token);
        }

        public ClaimsPrincipal? Validate(string token)
        {
            try
            {
                return _handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = _config.Issuer != null,
                    ValidIssuer = _config.Issuer,
                    ValidateAudience = _config.Audience != null,
                    ValidAudience = _config.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecurityKey
                }, out SecurityToken securityToken);
            } 
            catch (Exception)
            {
                return null;
            }
        }
    }
}
