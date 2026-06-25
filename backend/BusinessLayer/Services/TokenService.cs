using BusinessLayer.Interfaces;
using backend.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography; // <-- ADDED for Refresh Token
using System.Text;
using System; // <-- ADDED for Convert

namespace BusinessLayer.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _key;
        readonly string _issuer;
        readonly string _duration;
        
        public TokenService(IConfiguration configuration) 
        {
            _key = configuration["JWT:Key"] ?? "This is the alternate key tempdfjasdfakjsdfhaskjfhsakjdh";
            _issuer = configuration["JWT:Issuer"] ?? "Any Server";
            _duration = configuration["JWT:DurationInMinutes"] ?? "60";
        }

        // 1. Existing JWT Access Token Method
        public string CreateNewToken(TokenRequest request)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, request.UserId.ToString()),
                new Claim(ClaimTypes.Name,request.Username),
                new Claim(ClaimTypes.Role,request.Role.ToString())
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_duration)),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // 2. NEW: Generate Secure Refresh Token Method
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32]; // 32 bytes creates a 256-bit secure token
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}