using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace CrudforMedicshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        [HttpGet("Login")]   
        public IActionResult Loginpassword(string Login,string Password)
        {
            if (Login.Equals("Asror") && Password.Equals("1234"))
            {
                return Ok( CreateToken());
            }
            return Forbid();
        }
        private string CreateToken()
        {
            var Token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(20), 
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecretKey11111111111dasasdasdsadas")),
                SecurityAlgorithms.HmacSha256Signature
                ));
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
