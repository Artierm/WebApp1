using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (await UpdateLastDateLogin())
            {
                return View(_userManager.Users.ToList());
            }

            return Redirect("/Account/Login");
        }


        public async Task<bool> UpdateLastDateLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (!(user is null))
                {
                    user.LastLoginDate = DateTime.Now;
                }

                var userUpdate = await _userManager.UpdateAsync(user);
                if (!userUpdate.Succeeded)
                {
                    ModelState.AddModelError("", "Error update parameters");
                    return false;
                }
            }
            return true;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] isChecked)
        {
            foreach (var id in isChecked)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user != null)
                {
                    if (user.UserName == User.Identity.Name)
                    {
                        await _signInManager.SignOutAsync();

                    }
                    IdentityResult result = await _userManager.DeleteAsync(user);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Block(string[] isChecked)
        {
            foreach(var id in isChecked)
            {
                 var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    if (user.UserName == User.Identity.Name)
                    {
                        await _signInManager.SignOutAsync();

                    }
                    user.LockoutEnabled = false;
                    IdentityResult result = await _userManager.UpdateAsync(user);
                }
            }
        
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(string[] isChecked)
        {
            foreach (var id in isChecked)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.LockoutEnabled = true;
                    IdentityResult result = await _userManager.UpdateAsync(user);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
