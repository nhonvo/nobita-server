﻿using HDBank.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HDBank.Infrastructure.Services
{
    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // For API
        public string Authenticate(AppUser user, IList<string> roles)
        {
            var tokenClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.GivenName, user.FullName ?? "null"),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.UserData, user.AccountNo ?? "null"),
            new Claim(ClaimTypes.SerialNumber, user.IdentityNumber ?? "null")
        };
            foreach (var role in roles)
                tokenClaims.Add(new Claim(ClaimTypes.Role, role));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var tokenCredentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(tokenClaims),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = tokenCredentials,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        // For MVC
        public ClaimsPrincipal Validate(string token)
        {
            IdentityModelEventSource.ShowPII = true;
            TokenValidationParameters validationParameters = new()
            {
                ValidateLifetime = true,
                ValidAudience = _configuration["JWT:Issuer"],
                ValidIssuer = _configuration["JWT:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);

            return principal;
        }
        public DateTime GetExpireDate(string token)
        {
            JwtSecurityToken jwt = new(token);
            if (token == null)
                return DateTime.Now;
            return jwt.ValidTo.ToUniversalTime();
        }
    }
}