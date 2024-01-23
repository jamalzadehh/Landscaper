using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Practice.Models;
using Practice.ViewModels.AccountVMs;

namespace Practice.Areas.admin.Controllers
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

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user = new()
            {
                UserName = vm.UserName,
                Name = vm.Name,
                Surname = vm.Surname,
                Email = vm.Email,
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, item.Description);
                }
                return View(vm);
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            AppUser user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError(String.Empty, "Incorrect");
                }
                return View();
            }
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role = new(){ Name = "admin" };
            await _roleManager.CreateAsync(role);
            return RedirectToAction("Index", "Home");
        }

    }
}
