using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser1> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser1> userManager)
        {
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<TokenViewModel> Login(LoginModel1 model)
        {
            TokenViewModel tokenViewModel = new();
            var user = await _userManager.FindByNameAsync(model.Username);
            if(user == null)
            {
                tokenViewModel.StatusCode = 0;
                tokenViewModel.StatusMessage = "invalid username";
                return tokenViewModel;
            }
            var userroles=await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            foreach(var userrole in userroles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userrole));
            }
            tokenViewModel.AccessToken = Generatetoken(authClaims);
            tokenViewModel.RefreshToken = GenerateRefreshToken();
            tokenViewModel.StatusCode = 1;
            tokenViewModel.StatusMessage = "Success";
            var RefreshTokenValidityInDays = Convert.ToInt64(_configuration["JWTKey:RefreshTokenValidityInDays"]);
            user.RefreshToken = tokenViewModel.RefreshToken;
            user.RefreshTokenExpiryTime=DateTime.Now.AddDays(RefreshTokenValidityInDays);
            await _userManager.UpdateAsync(user);
            return tokenViewModel;
        }

        private string Generatetoken(IEnumerable<Claim> authClaims)
        {
            var authsigningkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
            var TokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInHour"]);
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration["JWTKey:ValidAudience"],
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(authsigningkey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(TokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<(int, string)> Registration(RegisteredModel1 model, string role)
        {
            var UserExists =await _userManager.FindByNameAsync(model.Username);
           
            if (UserExists != null)
            {
                return (0, "User is Already exist");
            }
            ApplicationUser1 user = new()
            {
                Email =model.Email,
                SecurityStamp=Guid.NewGuid().ToString(),
                UserName= model.Username,
                FirstName=model.Firstname,
                LastName=model.Lastname,
            };
            var CreatedUserResult=await _userManager.CreateAsync(user,model.Password);

           Console.WriteLine(CreatedUserResult);
            if (!CreatedUserResult.Succeeded) 
            {
                return (0, "UserCreation is Failed,please Check your user details and try again");
            }
            if(! await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return (1, "User is Created Successfully!");
        }
        public async Task<TokenViewModel>GetRefreshToken(GetRefreshTokenViewModel model)
        {
            TokenViewModel tokenViewModel = new();
            var Principal = GetPrincipalFromExpiredToken(model.AccessToken);
            string username = Principal.Identity.Name;
            var user=await _userManager.FindByNameAsync(username);
            if(user==null||user.RefreshToken!=model.RefreshToken||user.RefreshTokenExpiryTime<=DateTime.Now) 
            {
                tokenViewModel.StatusCode = 0;
                tokenViewModel.StatusMessage = "invalid access token or refresh token";
                return tokenViewModel;
            }
            var AuthClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

            };
            var newAccessToken = Generatetoken(AuthClaims);
            var NewRefreshToken = GenerateRefreshToken();
            user.RefreshToken = NewRefreshToken;
            await _userManager.UpdateAsync(user);
            tokenViewModel.StatusCode = 1;
            tokenViewModel.StatusMessage = "success";
            tokenViewModel.AccessToken= newAccessToken;
            tokenViewModel.RefreshToken = NewRefreshToken;
            return tokenViewModel;


        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var TokenValidationParametr = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"])),
                ValidateLifetime = false,

            };
            var Tokenhandler = new JwtSecurityTokenHandler();
            var principal=Tokenhandler.ValidateToken(token, TokenValidationParametr,out SecurityToken securityToken);
            if(securityToken is not JwtSecurityToken jwtSecurityToken||!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase)) 
            {
                throw new SecurityTokenException("invalid Token");
            }
            return principal;
        }

        public static string GenerateRefreshToken()
        {
            var randomnumber=new byte[64];
            using var rng=RandomNumberGenerator.Create();
            rng.GetBytes(randomnumber);
            return Convert.ToBase64String(randomnumber);
        }
    }
}
