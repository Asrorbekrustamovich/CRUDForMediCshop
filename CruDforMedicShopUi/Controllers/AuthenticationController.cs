﻿using CrudforMedicshop.Application.Interfaces;
using CrudforMedicshop.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CruDforMedicShopUi.Controllers
{
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
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("invalid playload");
                }
                var (status, message) = await _authService.Login(model);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> Register(RegisteredModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("invalid payload");
                }
                var (status, message) = await _authService.Registration(model, UserRoles.Admin);
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