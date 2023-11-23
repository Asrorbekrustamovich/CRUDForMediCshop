using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
using CrudforMedicshop.infrastructure.Dbcontext;
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
        private readonly IConfiguration _configuration;
       private readonly ApplicationDbcontext1 _dbcontext1;
        private ILogger<AuthenticationController> _logger;
        public RefreshTokenControlller(IAuthService authService, ILogger<AuthenticationController> logger, IConfiguration configuration,  ApplicationDbcontext1 dbcontext1)
        {
            _authService = authService;
            _logger = logger;
            _configuration = configuration;
            _dbcontext1 = dbcontext1;
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
            var user= _dbcontext1.UserforRefresh.Select(x=>x).Where(x=>x.UserName==username).FirstOrDefault();
            if(user == null)
            {
                return BadRequest("invalid username");
            }
           user.RefreshToken = null;
            _dbcontext1.Update(user);
            await _dbcontext1.SaveChangesAsync();
            return Ok("success");
        }
        [Authorize]
        [HttpPost("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var users = _dbcontext1.UserforRefresh;
            foreach(var user in users)
            {
                user.RefreshToken = null;
                await _dbcontext1.SaveChangesAsync();
            }
            return Ok("success");
        }
    }
}
