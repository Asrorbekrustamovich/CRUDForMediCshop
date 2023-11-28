using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using CrudforMedicshop.infrastructure.Services;

namespace CrudforMedicshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthentificationController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<Response<(Token,User)>>Register(UserDTO user)
        {
             return await _identityService.RegisterAsync(user);
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<Response<Token>>Login(Credential credential)
        {
            return await _identityService.LoginAsync(credential);
        }
        [HttpGet("Logout")]
        [AuthefrationAttributeFilter("Logout")]
        public async Task<(bool,RefreshToken)>Logout(string Refreshtoken)
        {
            return await _identityService.logoutAsync(Refreshtoken);
        }
        [HttpDelete("DeleteUser")]
        public async Task<Response<bool>>DeleteUser(int userid)
        {
            return await _identityService.DeleteUserAsync(userid);
        }
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<Response<Token>>RefreshToken(Token token)
        {
            return await _identityService.RefreshTokenAsync(token);
        }
        [HttpDelete("Delete")]
        public async Task<Response<bool>>Delete(int userid)
        {
            return await _identityService.DeleteUserAsync(userid);
        }

    }
}
