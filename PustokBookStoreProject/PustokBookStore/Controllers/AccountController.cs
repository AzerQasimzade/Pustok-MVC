using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using PustokBookStore.DAL;
using PustokBookStore.Models;
using PustokBookStore.ViewModels;

namespace PustokBookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(AppDbContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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
            if(existedUser is null)
            {
                existedUser=await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if(existedUser is null)
                {
                    ModelState.AddModelError(String.Empty, "UserName,Email or Password is incorrect");
                }
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(existedUser, loginVM.Password, loginVM.IsRemembered, true);
            if (result.Succeeded)
            {
                
            }


        }
    }
}
