using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public async Task<IActionResult>Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null) 
            {
                var result = await _roleManager.DeleteAsync(role);
            }

            return RedirectToAction("Index");
        }
    }
}
