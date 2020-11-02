using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public ActionResult<bool> GetIsAdmin()
        {
            if (!User.Identity.IsAuthenticated) return Ok(false);
            var currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var isAdmin = _userManager.IsInRoleAsync(currentUser, "Admin").Result;
            if (isAdmin)
            {
                return Ok(true);
            }
            else return Ok(false);
        }
    }
}
