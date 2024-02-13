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
using AutoMapper;
using Application.DTOs;

namespace Solvex_Workshop1.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _applicationContext;
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] LogIn logIn)
        {
            if (logIn == null)
            {
                return BadRequest();
            }


            var user = await _applicationContext.Users.FirstOrDefaultAsync(x => x.Username == logIn.Username);
            if (user == null)
            {
                return NotFound(new { Message = "User Not Found!" });
            }

            if (!PasswordHasher.VerifyPassword(logIn.Password, user.Password))
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
        public async Task<ActionResult> RegisterUser([FromBody] SignUp signUp)
        {
            if (signUp == null)
            {
                return BadRequest();
            }

            //Check username
            if (await CheckUserNameExistAsync(signUp.Username))
            {
                return BadRequest(new { Message = "Username already exist!" });
            }

            //Check email
            if (await CheckEmailExistAsync(signUp.Email))
            {
                return BadRequest(new { Message = "Email already exist!" });
            }

            var user = _mapper.Map<User>(signUp);


            user.Password = PasswordHasher.HashPassword(user.Password);
            user.Role = "User";
            user.Token = "";
            await _applicationContext.Users.AddAsync(user);
            await _applicationContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "User Registered!"
            });
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
