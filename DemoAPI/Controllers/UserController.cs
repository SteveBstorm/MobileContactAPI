using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DemoAPI.Models;
using DemoAPI.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private ITokenManager _tokenManager;

        public UserController(IUserService userService, ITokenManager tokenManager)
        {
            _userService = userService;
            _tokenManager = tokenManager;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginForm form)
        {
            User currentUser = _userService.Login(form.Email, form.Password);

            string token = _tokenManager.GenerateJWT(currentUser);

            UserWithToken u = new UserWithToken
            {
                Id = currentUser.Id,
                Email = currentUser.Email,
                IsAdmin = currentUser.IsAdmin,
                Token = token
            };

            return Ok(u);
        }

        [HttpPost("register")]
        public IActionResult Register(LoginForm form)
        {
            _userService.Register(form.Email, form.Password, false);
            return Ok();
        }
    }
}
