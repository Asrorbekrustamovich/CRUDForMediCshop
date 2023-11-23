using CrudforMedicshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(int, string)> Registration(RegisteredModel1 model);
        Task<TokenViewModel> Login(LoginModel1 model);
        Task<TokenViewModel>GetRefreshToken(GetRefreshTokenViewModel model);
    }
}
