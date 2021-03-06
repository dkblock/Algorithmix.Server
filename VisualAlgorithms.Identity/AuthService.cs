﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Identity
{
    public class AuthService
    {
        private readonly IdentitySettings _identitySettings;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly byte[] _secret;

        public AuthService(IOptions<IdentitySettings> identitySettings)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _secret = Encoding.ASCII.GetBytes(identitySettings.Value.Secret);
        }

        public SecurityToken Authenticate(ApplicationUserEntity user, IdentityRole role)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.PrimarySid, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, role.Name)
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature)
            };

            return _tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
