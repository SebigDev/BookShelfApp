using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShelfApp.Managers;
using BookShelfApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookShelfApp.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Register([FromBody] RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userReg = await _userManager.RegisterUser(viewModel);
            return Ok(userReg);

        }

        public async Task<IActionResult> Login([FromBody] LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userLogin = await _userManager.Login(viewModel);
            return Ok(userLogin);

        }

        public async Task<IActionResult> GetUser(long userId)
        {
            if (userId <= 0)
            {
                return BadRequest();
            }
            var user = await _userManager.GetUser(userId);
            return Ok(user);

        }
    }
}
