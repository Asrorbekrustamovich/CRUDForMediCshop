using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Application.Interfaces
{
    public interface ITokenService
    {
        Task<Token> GenerateTokenAsync(User user);
        Task<string> GenerateRefreshTokenAsync();
        Task<Token> GetNewTokenFromExpiredToken(Token tokens);
    }
}
