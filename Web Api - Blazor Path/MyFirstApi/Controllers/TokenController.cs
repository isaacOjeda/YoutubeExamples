using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyFirstApi.Dto.Token;
using MyFirstApi.Helpers;
using MyFirstApi.Models;

namespace MyFirstApi.Controllers
{
    public class TokenController : ApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public TokenController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Authorize([FromBody] TokenLoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user is null)
            {
                return Forbid(AppConstants.JwtScheme);
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordCheck)
            {
                return Forbid(AppConstants.JwtScheme);
            }

            var now = DateTime.UtcNow;
            var seconds = DateTimeOffset.Now.ToUnixTimeSeconds();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, seconds.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Sid, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
               issuer: AppConstants.Issuer,
               audience: AppConstants.Audience,
               claims: claims,
               notBefore: now,
               expires: now.AddMinutes(AppConstants.JwtTokenExpiration),
               signingCredentials: credentials);

            var response = new TokenLoginResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt)
            };

            return Ok(response);
        }
    }
}