using Database;
using Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using Application.Helpers;

namespace Solvex_Workshop1.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _applicationContext;

        public UserController(ApplicationDbContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }


            var user = await _applicationContext.Users.FirstOrDefaultAsync(x => x.Username == userObj.Username);
            if (user == null)
            {
                return NotFound(new { Message = "User Not Found!" });
            }

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }

            user.Token = CreateJwt(user);

            return Ok(new
            {
                Token = user.Token,
                Message = "Login Success!"
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }

            //Check username
            if (await CheckUserNameExistAsync(userObj.Username))
            {
                return BadRequest(new { Message = "Username already exist!" });
            }

            //Check email
            if (await CheckEmailExistAsync(userObj.Email))
            {
                return BadRequest(new { Message = "Email already exist!" });
            }

            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            await _applicationContext.Users.AddAsync(userObj);
            await _applicationContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "User Registered!"
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _applicationContext.Users.ToListAsync());
        }


        #region "methods"
        private async Task<bool> CheckUserNameExistAsync(string username)
        {
            return await _applicationContext.Users.AnyAsync(x => x.Username == username);
        }

        private async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _applicationContext.Users.AnyAsync(x => x.Email == email);
        }

        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authentication");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
        #endregion

    }
}
