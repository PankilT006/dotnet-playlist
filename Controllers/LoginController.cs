using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.DTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("LoginCheck")]
        public ActionResult LoginCheck(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ModelState is not valid Please check the data you have entered");
            }
            LoginResponseDTO response = new()
            {
                Username = model.Username
            };
            if (model.Username == "Pankil" && model.Password == "Pankil1905")
            {
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecret"));
//                 if (string.IsNullOrEmpty(key.ToString()))
//                 {
// Console.WriteLine("JWTSecret is not set in configuration.");
//                     return StatusCode(StatusCodes.Status500InternalServerError, "Server configuration error");
//                 }
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                   {
                    //Username  
                    new Claim(ClaimTypes.Name, model.Username),
                    //Role
                    new Claim(ClaimTypes.Role, "Admin")
                   }),
                    Expires = DateTime.UtcNow.AddSeconds(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.Token = tokenHandler.WriteToken(token);
            }
            else
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(response);
        }
    }
}
