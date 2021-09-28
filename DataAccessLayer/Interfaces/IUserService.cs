using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserService
    {
        void Register(string Email, string Password, bool IsAdmin);
        User Login(string Email, string Password);
    }
}
