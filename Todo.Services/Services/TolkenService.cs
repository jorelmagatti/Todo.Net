using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Todo.Models.Dtos;
using Todo.Services.Interfaces;

namespace Todo.Services.Services
{
    public class TolkenService : Service
    {
        #region Private parameters
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public TolkenService(ILogger logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(logger, httpContextAccessor)
        {
            _configuration = configuration;
        }
        #endregion

        #region Public Methods
        public string GenerateTolken(UserDto user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var tolken = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(tolken);
            }
            catch (Exception ex)
            {
                AddLogToAmbient(ex);
                throw;
            }
        }
        #endregion
    }
}
