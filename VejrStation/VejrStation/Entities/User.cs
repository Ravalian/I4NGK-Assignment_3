using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity;

namespace VejrStation.Entities
{
    public class User 
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(64)]
        public string FirstName { get; set; }
        [MaxLength(64)]
        public string LastName { get; set; }
        [MaxLength(256)]
        public string Username { get; set; }
        [MaxLength(256)]
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
