using AutoMapper;
using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using CrudforMedicshop.infrastructure.Dbcontext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly ITokenService _tokenService;
        private readonly Mydbcontext _mydbcontext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly int _refreshTokenLifetime;

        public IdentityService(ITokenService tokenService, Mydbcontext mydbcontext, IMapper mapper, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _mydbcontext = mydbcontext;
            _mapper = mapper;
            _configuration = configuration; 
            _refreshTokenLifetime = int.Parse(_configuration["JWTKey:RefreshTokenValidityInMinutes"]);
        }


        public Task<Response<bool>> DeleteUserAsync(int Userid)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsValidRefreshToken(string RefreshToken, int userid)
        {
            var res = _mydbcontext.RefreshTokens.Where(x => x.UserId.Equals(userid) && x.RefreshTokenValue.Equals(RefreshToken));
            RefreshToken? refreshTokenEntity;
            if (res.Count() != 1) 
            {
                return false;
            }
            refreshTokenEntity=res.First();
            if (refreshTokenEntity.ExpireTime<DateTime.Now)
            {
                return false;
            }
            return true;

        }

        public  async Task<Response<Token>> LoginAsync(Credential credential)
        {
            credential.Password=_tokenService.ComputeSha256hash(credential.Password);
            User user=_mydbcontext.Users.Where(x=>x.Username.Equals(credential.Username)&&x.Password.Equals(credential.Password)).FirstOrDefault();
            if (user == null)
            {
                return new("user not found",404);
            }
            Token token = await _tokenService.GenerateTokenAsync(user);
            bool issuccess = await SaveRefreshToken(token.RefreshToken, user);
            return issuccess ? new(token) : new("faild to save refresh token");
        }

        public  async Task<bool> logoutAsync(string RefreshToken)
        {
            var refresh=_mydbcontext.RefreshTokens.Where(x=>x.RefreshTokenValue.Equals(RefreshToken)).FirstOrDefault();
            if(refresh==null)
            {
                return false;
            }
            refresh.RefreshTokenValue = null;
            _mydbcontext.RefreshTokens.Update(refresh);
            _mydbcontext.SaveChanges();
            return true;
        }

        public async Task<Response<Token>> RefreshTokenAsync(Token token)
        {
            User user = await _tokenService.GetClaimsFromExpiredToken(token.AccessTokenk);
            if (!await IsValidRefreshToken(token.RefreshToken,user.Id))
            {
                return new("refresh token is invalid");
            }
            Token tokennew= await _tokenService.GenerateTokenAsync(user);
            bool issuccess = await SaveRefreshToken(tokennew.RefreshToken, user);
            return issuccess? new(tokennew) : new("Failed");
        }

        public async Task<Response<(Token,User)>> RegisterAsync(UserDTO userDTO)
        {   User user=_mapper.Map<User>(userDTO);
            var isExistuser = _mydbcontext.Users.Where(x => x.Username.Equals(userDTO.Username));
            if(isExistuser.Count()>0)
            {
                return new("user already exist");
            }
            user.Password = _tokenService.ComputeSha256hash(user.Password);
            await _mydbcontext.Users.AddAsync(user);
            int effectrows= _mydbcontext.SaveChanges();
            if(effectrows<=0)
            {
                return new("operation failed");
            }
            Token token =  await _tokenService.GenerateTokenAsync(user);
            bool issuccess =await SaveRefreshToken(token.RefreshToken,user);
            return issuccess ? new((token, user)) : new("failed");
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

        public async Task<bool> SaveRefreshToken(string refreshToken,User user)
        {
             if(string.IsNullOrEmpty(refreshToken))
             {
                return false;
             }
            RefreshToken? refreshTokenEntity;
            var res = _mydbcontext.RefreshTokens.Where(x => x.UserId.Equals(user.Id) && x.RefreshTokenValue.Equals(refreshToken));
            if(res.Count()==0)
            {
                refreshTokenEntity = new()
                {
                    ExpireTime = DateTime.Now.AddMinutes(_refreshTokenLifetime),
                    UserId = user.Id,
                    RefreshTokenValue = refreshToken
                };
                await _mydbcontext.RefreshTokens.AddAsync(refreshTokenEntity);
              
            }
            else if(res.Count()==1) 
            {
                refreshTokenEntity=res.First();
                refreshTokenEntity.RefreshTokenValue = refreshToken;
                refreshTokenEntity.ExpireTime=DateTime.Now.AddMinutes(_refreshTokenLifetime);
                _mydbcontext.RefreshTokens.Update(refreshTokenEntity);
            }

            else 
            {
                return false;
            }

           
            int rows= await _mydbcontext.SaveChangesAsync();
            return rows > 0;
        }
    }
}
