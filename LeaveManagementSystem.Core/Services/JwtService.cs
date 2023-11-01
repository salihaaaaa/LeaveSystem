using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LeaveManagementSystem.Core.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public AuthenticationResponse CreateJwtTokenAsync(ApplicationUser user)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

            IList<string> roles = _userManager.GetRolesAsync(user).Result;

            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),         //Subject (user id)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  //JWT unique id
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),         //Issued at (date and time of token generation)
                new Claim(JwtRegisteredClaimNames.Iat, user.Email),                 //Unique name identifier of the user (email)  
                new Claim(JwtRegisteredClaimNames.Name, user.Name),                  //Name of the user
                new Claim("role", string.Join(",", roles))
            };

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiration,
                signingCredentials: signingCredentials);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            string token = tokenHandler.WriteToken(tokenGenerator);

            return new AuthenticationResponse()
            {
                Token = token,
                Email = user.Email,
                Name = user.Name,
                Expiration = expiration,
                Role = string.Join(",", roles) 
            };

        }
    }
}
