using System;
using System.Text;
using System.Security.Claims;
using backend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Services.AuthService
{
	public class ServiceAuth
    {
        private readonly IConfiguration _configuration;

        public ServiceAuth(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
                new Claim(ClaimTypes.Name, user.firstName!),
                new Claim(ClaimTypes.Role, "User")
            };
            // To create our JWT (Token) we need 3 things: a key, a signature and the data that we are storing. 

            // Creating the Key
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            // Getting our credentials (Probably to create a unique signature)
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

            // Create or token with some info
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            // Writing the token
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string ValidateAndGetIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!)),
            };

            ClaimsPrincipal claimsPrincipal;
            SecurityToken validatedToken;

            try
            {
                // Decode the token
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (SecurityTokenException ex)
            {
                return "Invalid Token";
            }

            // get the id from the token
            //claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}

