﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.Utilities.Enums;
using PustokBookStore.ViewModels;

namespace PustokBookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if(!ModelState.IsValid)return View();

            AppUser user = new AppUser
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Gender = registerVM.Gender,
                Surname = registerVM.Surname,
                UserName = registerVM.Fullname

            };
            IdentityResult identityResult= await _userManager.CreateAsync(user,registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                return View();
               
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser existedUser = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (existedUser is null)
            {
                existedUser = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (existedUser is null)
                {
                    ModelState.AddModelError(String.Empty, "UserName,Email or Password is incorrect");
                    return View();

                }
            }
            var result = await _signInManager.PasswordSignInAsync(existedUser, loginVM.Password, loginVM.IsRemembered, true);
			if (result.IsLockedOut)
			{
				ModelState.AddModelError(String.Empty, "Your Account Blocked because of Fail attempts Please try later");
			}
			if (!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "UserName,Email or Password is incorrect");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout(LoginVM loginVM)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
		}
        public async Task<IActionResult> CreateRoles()
        {
            foreach (var role in Enum.GetValues(typeof(UserRoleHelper)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    });
                }          
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
