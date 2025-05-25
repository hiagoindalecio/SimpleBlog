using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleBlog.Application.DTOs;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleBlog.Infrastructure.Authentication
{
    public class JwtTokenGenerator(IOptions<JwtSettings> settings) : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings = settings.Value;

        public AuthResponseDto GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.Name),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Exp, DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes).ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims,
                null,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes),
                signingCredentials
            );

            var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto(stringToken, user.Name, user.Email);
        }
    }

}
