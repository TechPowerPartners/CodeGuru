using botovskixAPI.Models;
using botovskixAPI.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace botovskixAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public ApplicationDbContext Context;

        public UserController(ApplicationDbContext context)
        {
            Context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(Users users)
        {
            var findUser = Context.Users.FirstOrDefault(u => u.Name == users.Name);
            

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ismailismailismailismailismailismailismail"));
            var loginCredential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "Swagger",
                audience: "Sms",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: loginCredential);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(tokenString);

        }

        [HttpPost("registration")]
        public IActionResult Registration(UserDto user)
        {
            if (user.Validator == 765123)
            {
                var findUser = Context.Users.FirstOrDefault(u => u.Name == user.Name);

                if (findUser != null)
                {
                    findUser.Name = user.Name;
                    findUser.Password = user.Password;
                    Context.SaveChangesAsync();
                    return StatusCode(409, $"User already exist");
                }
                
                Context.Users.Add(new Users
                {
                    Name = user.Name,
                    Password = user.Password,
                });
                Context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("низя");
        }
    }
}
