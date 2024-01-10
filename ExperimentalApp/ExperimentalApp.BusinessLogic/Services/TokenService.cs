using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.Core.Models;
using ExperimentalApp.DataAccessLayer.DBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExperimentalApp.BusinessLogic.Services
{
    /// <summary>
    /// Represents token service
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Token constructor with neccessary params 
        /// </summary>
        /// <param name="configuration">Gets application configuration</param>
        /// <param name="serviceScopeFactory">Gets services scope factory to get access to application services.</param>
        public TokenService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }    
    
        /// <inheritdoc/>
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <inheritdoc/>
        public async Task<bool> IsTokenInBlackListAsync(string token)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DvdRentalContext>();

                return await dbContext.BlacklistedTokens.AnyAsync(t => t.Token == token);
            }       
        }

        /// <inheritdoc/>
        public async Task SetTokenAsInvalidAsync(string token)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DvdRentalContext>();

                var blacklistedToken = new BlackListedToken { Token = token };
                dbContext.BlacklistedTokens.Add(blacklistedToken);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
