using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using api.Utils;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("api/Authentication")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApiDbContext _db;

        public AuthenticationController(SignInManager<ApplicationUser> signInManager,
                UserManager<ApplicationUser> userManager,
                ApiDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return BadRequest("username and password may not be empty");

            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return BadRequest("Invalid username or password.");
            }
            var user = await _userManager.Users
                .SingleAsync(i => i.UserName == username);
            if (!user.IsEnabled)
            {
                return BadRequest("Invalid username or password.");
            }
            var response = GetLoginToken.Execute(user, _db);
            return Ok(response);
        }
    }
}