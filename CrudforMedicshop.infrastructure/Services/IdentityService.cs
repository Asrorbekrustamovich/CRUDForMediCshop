using AutoMapper;
using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using CrudforMedicshop.infrastructure.Dbcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.infrastructure.Services
{
    public class IdentityService : IIdentityService
    {   private readonly ITokenService _tokenService;
        private readonly Mydbcontext _mydbcontext;
        private readonly IMapper _mapper;

        public IdentityService(ITokenService tokenService, Mydbcontext mydbcontext, IMapper mapper)
        {
            _tokenService = tokenService;
            _mydbcontext = mydbcontext;
            _mapper = mapper;
        }

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

        public async Task<Response<(Token,User)>> RegisterAsync(UserDTO userDTO)
        {   User user=_mapper.Map<User>(userDTO);
            await _mydbcontext.Users.AddAsync(user);
            int effectrows= _mydbcontext.SaveChanges();
            if(effectrows<=0)
            {
                return new("operation failed");
            }
            Token token =  await _tokenService.GenerateTokenAsync(userDTO);
            return new((token, user));
        }
    }
}
