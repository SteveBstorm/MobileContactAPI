using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Models
{
    public class UserWithToken
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public string Token { get; set; }
    }
}
