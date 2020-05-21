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

namespace VejrStation.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDBContext _context;
        private IUserService _userService;
        private readonly AppSettings _appSettings;

        public UsersController(MyDBContext context, IUserService userService, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetUser", new {id = user.Id}, user);
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        //public IActionResult Authenticate([FromBody] User userParam)
        public async Task<ActionResult<TokenDto>> Authenticate([FromBody]User userParam)
        {
            //****1st try for authentication****//
            var user = await _context.Users.SingleOrDefaultAsync(a => a.Username == userParam.Username && a.Password == userParam.Password);

            if (user != null)
            {
                var token = new TokenDto();
                token.JWT = GenerateToken(user);
                return token;
            }
            else
            {
                return BadRequest(new { message = "Username or password incorrect" });
            }

            //****2nd try for authentication****//
            //var user = _userService.Authenticate(userParam.Username, userParam.Password);

            //if (user == null)
            //    return BadRequest(new { message = "Username or password incorrect" });

            //return Ok(user);
        }


        //bliver anvendt til 1st try Authentication
        private string GenerateToken(User user)
        {
            //Claim roleClaim;
            //if (isSomething)
            //    roleClaim = new Claim("Role", "Admin");
            //else
            //    roleClaim = new Claim("Role", "Worker");

            var claims = new Claim[]
            {
                new Claim("Username", user.Username),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),     
                // roleClaim,
                new Claim("UserId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}