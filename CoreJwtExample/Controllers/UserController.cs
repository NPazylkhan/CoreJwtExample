using CoreJwtExample.IRepository;
using CoreJwtExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace CoreJwtExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;
        IUserRepository _userRepository;

        public UserController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromBody] User model)
        {
            try
            {
                model = await _userRepository.Save(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Signin/{username}/{password}")]
        public async Task<IActionResult> Signin(string username, string password)
        {
            try
            {
                User model = new User()
                {
                    UserName = username,
                    Password = password
                };
                var user = await AuthenticationUser(model);
                if (user.UserId == 0)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, "Invalid user");
                }

                user.Token = GenerateToken(model);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private string GenerateToken(User model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                            _configuration["Jwt:Issuer"],
                                            null,
                                            expires: DateTime.Now.AddMinutes(120),
                                            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> AuthenticationUser(User user)
        {
            return await _userRepository.GetByUserNamePassword(user);
        }
    }
}
