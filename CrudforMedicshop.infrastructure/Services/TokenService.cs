using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.infrastructure.Services
{
    public class TokenService : ITokenService
    {
        public Task<string> GenerateRefreshTokenAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Token> GenerateTokenAsync(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim("id",user.Id.ToString())
            };
            Token token = new Token()
            {
                RefreshToken = await GenerateRefreshTokenAsync()
            };
        }

        public Task<Token> GetNewTokenFromExpiredToken(Token tokens)
        {
            throw new NotImplementedException();
        }
    }
}
