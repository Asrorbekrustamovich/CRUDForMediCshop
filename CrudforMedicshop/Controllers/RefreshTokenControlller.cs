using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrudforMedicshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenControlller : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser1> _userManager;

        public RefreshTokenControlller(IAuthService authService, ILogger<AuthenticationController> logger, IConfiguration configuration, UserManager<ApplicationUser1> userManager)
        {
            _authService = authService;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult>RefreshToken(GetRefreshTokenViewModel model)
        {
            try
            {
                if(model is null)
                {
                    return BadRequest("invalid client request");
                }
                var result = await _authService.GetRefreshToken(model);
                if(result.StatusCode==0)
                {
                    return BadRequest(result.StatusMessage);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize]
        [HttpPost("revoke/{username}")]
        public async Task<IActionResult>Revoke(string username)
        {
            var user=await _userManager.FindByNameAsync(username);
            if(user == null)
            {
                return BadRequest("invalid username");
            }
           user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return Ok("success");
        }
        [Authorize]
        [HttpPost("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach(var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
            return Ok("success");
        }
    }
}
