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
    public class IdentityService : IIdentityService
    {
        public Task<Response<bool>> DeleteUserAsync(int Userid)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Token>> LoginAsync(Credential credential)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> logoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Response<Token>> RefreshTokenAsync(Token token)
        {
            throw new NotImplementedException();
        }

        public Task<Response<(Token,User)>> RegisterAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
