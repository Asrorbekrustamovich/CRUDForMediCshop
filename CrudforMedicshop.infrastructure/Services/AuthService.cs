using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using CrudforMedicshop.infrastructure.Dbcontext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CrudforMedicshop.infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbcontext1 _dbcontext1;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IConfiguration configuration,
            ILogger<AuthService> logger,
            ApplicationDbcontext1 dbcontext1)
        {
            _configuration = configuration;
            _logger = logger;
            _dbcontext1 = dbcontext1;
        }

        public async Task<TokenViewModel> Login(LoginModel1 model)
        {
            var user = _dbcontext1.UserforRefresh.Select(x => x).Where(x => x.username == model.Username).First();
            TokenViewModel tokenViewModel = new();
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
            tokenViewModel.AccessToken = Generatetoken(authClaims);
            tokenViewModel.RefreshToken = GenerateRefreshToken();
            tokenViewModel.StatusCode = 1;
            tokenViewModel.StatusMessage = "Success";

            var refreshTokenValidityInDays = Convert.ToInt64(_configuration["JWTKey:RefreshTokenValidityInDays"]);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);

            user.RefreshToken = tokenViewModel.RefreshToken;
            user.AccessToken = tokenViewModel.AccessToken;
            _dbcontext1.UserforRefresh.Update(user);
            _dbcontext1.SaveChanges();
            return tokenViewModel;
        }



        private string Generatetoken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
            var tokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInHour"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration["JWTKey:ValidAudience"],
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTimeInHour),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<(int, string)> Registration(RegisteredModel1 model)
        {
            var userExists = _dbcontext1.UserforRefresh.Select(x => x).Where(x => x.username == model.Username).FirstOrDefault();

            if (userExists != null)
            {
                return (0, "User is already exist");
            }

            ApplicationUser1 user = new()
            {
                username = model.Username,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                AccessToken = GenerateAccessToken(),
                RefreshToken = GenerateRefreshToken()
            };

            var createdUserResult = _dbcontext1.UserforRefresh.Add(user);
            _dbcontext1.SaveChanges();
            if (createdUserResult == null)
            {
                return (0, "User creation failed. Please check your user details and try again");
            }

            return (1, "User is created successfully!");
        }

        private string GenerateAccessToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var accessToken = new string(Enumerable.Repeat(chars, 32)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return accessToken;
        }

        public async Task<TokenViewModel> GetRefreshToken(GetRefreshTokenViewModel model)
        {
            TokenViewModel tokenViewModel = new();
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);
            string username = principal.Identity.Name;
            var user = await _dbcontext1.UserforRefresh.FindAsync(username);

            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                tokenViewModel.StatusCode = 0;
                tokenViewModel.StatusMessage = "Invalid access token or refresh token";
                return tokenViewModel;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var newAccessToken = Generatetoken(authClaims);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;

            _dbcontext1.UserforRefresh.Update(user);
            _dbcontext1.SaveChanges();

            tokenViewModel.StatusCode = 1;
            tokenViewModel.StatusMessage = "Success";
            tokenViewModel.AccessToken = newAccessToken;
            tokenViewModel.RefreshToken = newRefreshToken;

            return tokenViewModel;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
