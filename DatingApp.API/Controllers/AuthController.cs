using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo repo;
        private readonly IConfiguration config;

        public AuthController(IAuthRepo repo, IConfiguration config)
        {
            this.repo = repo;
            this.config = config;
        } 

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegister register)
        {
            register.Username = register.Username.ToLower();

            if(await repo.UserExsist(register.Username))
            {
                return BadRequest("User already exsist.");
            }

            var userToCreate = new Models.User
            {
                Username = register.Username
            };

            var createUser = await repo.Register(userToCreate, register.Password);
            
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDto)
        {
            //Get token after successful login
            var userFromRepo = await repo.Login(userLoginDto.Username.ToLower(), userLoginDto.Password);
            
            if(userFromRepo == null)
            {
                return Unauthorized();
            }

            // var claimsCheck = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name);
            // var usern = (claimsCheck == null ? string.Empty : claimsCheck.Value);
            
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                 new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHabdler = new JwtSecurityTokenHandler();

            var token = tokenHabdler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHabdler.WriteToken(token)
            });
        }
    }
}