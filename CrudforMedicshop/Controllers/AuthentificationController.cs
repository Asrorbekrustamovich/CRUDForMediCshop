using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CrudforMedicshop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AuthentificationController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthentificationController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpPost]
        public async Task<Response<(Token,User)>>Register(UserDTO user)
        {
             return await _identityService.RegisterAsync(user);
        }
        [HttpPost]
        public async Task<Response<Token>>Login(Credential credential)
        {
            return await _identityService.LoginAsync(credential);
        }
        [HttpGet]
        public async Task<Response<bool>>Logout(string Refreshtoken)
        {
            return await _identityService.logoutAsync(Refreshtoken);
        }
        [HttpDelete]
        public async Task<Response<bool>>DeleteUser(int userid)
        {
            return await _identityService.DeleteUserAsync(userid);
        }

    }
}
