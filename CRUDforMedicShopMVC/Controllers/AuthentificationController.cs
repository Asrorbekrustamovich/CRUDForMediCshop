using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using CrudforMedicshop.infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRUDforMedicShopMVC.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthentificationController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDTO user)
        {
            await _identityService.RegisterAsync(user);
            return View();
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Credential credential)
        {
             await _identityService.LoginAsync(credential);
            return View();  
        }
        [HttpGet("Logout")]
        [AuthefrationAttributeFilter("Logout")]
        public async Task<IActionResult> Logout(string Refreshtoken)
        {
            await _identityService.logoutAsync(Refreshtoken);
            return View();
        }
        [HttpDelete("DeleteUser")]
        [AuthefrationAttributeFilter("DeleteUser")]
        public async Task<Response<bool>> DeleteUser(int userid)
        {
            return await _identityService.DeleteUserAsync(userid);
        }
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<Response<Token>> RefreshToken(Token token)
        {
            return await _identityService.RefreshTokenAsync(token);
        }
        [HttpDelete("Delete")]
        [AuthefrationAttributeFilter("Delete")]
        public async Task<Response<bool>> Delete(int userid)
        {
            return await _identityService.DeleteUserAsync(userid);
        }
    }
}
