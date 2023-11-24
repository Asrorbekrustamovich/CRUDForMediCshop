using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<(Token,User)>> RegisterAsync(UserDTO user);
        Task<Response<Token>>LoginAsync(Credential credential);
        Task<Response<bool>> logoutAsync();
        Task<Response<Token>>RefreshTokenAsync(Token token);
        Task<Response<bool>> DeleteUserAsync(int Userid);
    }
}
