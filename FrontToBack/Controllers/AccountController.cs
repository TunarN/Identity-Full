using FrontToBack.Helper;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult>Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new();
            user.Email = register.Email;
            user.Fullname= register.Fullname;
            user.UserName = register.Username;
            user.IsActive = true;
            IdentityResult result= await _userManager.CreateAsync(user,register.Password);
            if (!result.Succeeded) 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }

            //role
            await _userManager.AddToRoleAsync(user,RoleEnums.Admin.ToString());

  

            return RedirectToAction("login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult>Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
            if (user==null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);

                if (user == null)
                {
                    ModelState.AddModelError("", "UserName or Email or Password is Wrong");
                    return View(loginVM);
                }
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Account is Blocked");
                return View(loginVM);
            }

          var  result = await _signInManager.PasswordSignInAsync(user,loginVM.Password,loginVM.RememberMe,true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account is Blocked");
                return View(loginVM);
            }

            if (!result.Succeeded)
            {

                ModelState.AddModelError("", "UserName or Email or Password is Wrong");
                return View(loginVM);
            }

            //sign in

            if (await _userManager.IsInRoleAsync(user, RoleEnums.Admin.ToString()))
            {
                return RedirectToAction("index", "dashboard", new { Area = "AdminArea" });
            }

            //if (ReturnUrl!= null)
            //{
            //    return Redirect(ReturnUrl);
            //}

            await _signInManager.SignInAsync(user, true);


            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("login");
        }

        //public async Task<IActionResult> CreateRole()
        //{
        //    foreach (var item in Enum.GetValues(typeof(RoleEnums)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(item.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
        //        }
        //    }
        //    return Content("Role added");
        //}
    }
}
