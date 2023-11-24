using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<Token> GenerateTokenAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<Token> GetNewTokenFromExpiredToken(Token tokens)
        {
            throw new NotImplementedException();
        }
    }
}
