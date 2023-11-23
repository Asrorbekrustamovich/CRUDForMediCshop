using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudforMedicshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel1 model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("invalid playload");
                }
                var result = await _authService.Login(model);
                if (result.StatusCode == 0)
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
        [HttpPost("Registration")]
        public async Task<IActionResult> Register(RegisteredModel1 model)
        {
            try
            {
                var (status, message) = await _authService.Registration(model);
                Console.WriteLine(status);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return CreatedAtAction(nameof(Register), model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }



}
