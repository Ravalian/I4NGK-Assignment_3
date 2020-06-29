using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VejrStation.Database;
using VejrStation.Entities;
using VejrStation.Utilities;
using static BCrypt.Net.BCrypt;

namespace VejrStation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDBContext _context;
        private readonly AppSettings _appSettings;
        public const int BcryptWorkfactor = 10;

        public UsersController(MyDBContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            //Makes sure the password is not returned for show
            user.Password = null;

            return user;
        }

        //CreateUser
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            User tempUser = user;
            tempUser.Password = HashPassword(user.Password, BcryptWorkfactor);
            _context.Users.Add(tempUser);
            await _context.SaveChangesAsync();

            var token = new TokenDto();
            token.JWT = GenerateToken(tempUser);

            return CreatedAtAction("GetUser", new {id = tempUser.Id}, token);
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        //public IActionResult Authenticate([FromBody] User userParam)
        public async Task<ActionResult<TokenDto>> Authenticate([FromBody]User userParam)
        {
            //****1st try for authentication****//
            var user = await _context.Users.SingleOrDefaultAsync(a => a.Username == userParam.Username);

            if (user != null)
            {
                var validPwd = Verify(userParam.Password, user.Password);
                if (validPwd)
                {
                    var token = new TokenDto();
                    token.JWT = GenerateToken(user);
                    return token;
                }
            }
            return BadRequest(new { message = "Username or password incorrect" });
        }


        //bliver anvendt til 1st try Authentication
        private string GenerateToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim("UserName", user.Username),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim("UserId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(2)).ToUnixTimeSeconds().ToString())
            };

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha384)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}