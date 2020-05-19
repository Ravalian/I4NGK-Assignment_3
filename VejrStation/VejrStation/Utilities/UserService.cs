using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VejrStation.Database;
using VejrStation.Entities;

namespace VejrStation.Utilities
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        //IEnumerable<User> GetAll();
    }
    
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly MyDBContext _context;

        public UserService(MyDBContext context, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            //Checking if user exists in database
            var user = _context.Users.SingleOrDefault(a => a.Username == username && a.Password == password);

            //return null if user not found
            if (user == null) return null;

            //Generates a JWT when user exists in database
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                //Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);



            //Makes sure the password is not returned for show
            user.Password = null;

            return user;
        }

        //public IEnumerable<User> GetAll()
        //{
        //    return _context.Users.Select(x =>
        //    {
        //        x.Password = null;
        //        return x;
        //    });
        //}
    }
}
