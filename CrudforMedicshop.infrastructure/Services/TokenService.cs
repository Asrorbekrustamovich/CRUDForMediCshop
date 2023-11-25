﻿using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.infrastructure.Services
{
    public class TokenService : ITokenService
    {   private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ComputeSha256hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {

                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public async Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[64];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return await Task.FromResult(Convert.ToBase64String(randomNumber));
        }



        public async Task<Token> GenerateTokenAsync(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim("id",user.Id.ToString())
            };
           foreach (var role in user.Roles)
            {
                //claims.Add(new Claim(ClaimTypes.Role,role.Name));
                foreach (var permission in role.Permissions)
                {
                    claims.Add(new Claim("permission", permission.name));
                }
            }
          
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            double accessTokenLife = double.Parse(_configuration["JWTKey:TokenExpiryTimeInMinutes"]);
            var token1 = new JwtSecurityToken(claims:claims, expires: DateTime.Now.AddMinutes(accessTokenLife),signingCredentials:credentials);
            string accessToken= new JwtSecurityTokenHandler().WriteToken(token1);
            Token token = new Token()
            {
                RefreshToken = await GenerateRefreshTokenAsync(),
                AccessTokenk= accessToken
                
            };
            return token;
        }

        public async Task<User> GetClaimsFromExpiredToken(string accesstoken)
        {
            var handler=new JwtSecurityTokenHandler();
            var jsontoken = handler.ReadToken(accesstoken);
            var claims=(jsontoken as JwtSecurityToken)?.Claims;
            var userclaims=claims?.ToArray();
            User user= new()
            {
                Id = int.Parse(userclaims.First(x => x.Type.Equals("id")).Value),
                Name = userclaims.First(x=>x.Type.Equals(ClaimTypes.NameIdentifier)).Value
            };
            return user;
        }

        public Task<Token> GetNewTokenFromExpiredToken(Token tokens)
        {
            throw new NotImplementedException();
        }
    }
}
