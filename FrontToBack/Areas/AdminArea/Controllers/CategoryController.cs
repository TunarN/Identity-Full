using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View(_appDbContext.Category.ToList());
        }

        public IActionResult Detail(int id)
        {
            if (id == null) return NotFound();
            Category category = _appDbContext.Category.SingleOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
            
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid) return View();
            bool isExist = _appDbContext.Category.Any(c => c.Name.ToLower() == category.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name","bu adli categori movcuddur");
                return View();
            }
            Category newCategory = new()
            {
                Name = category.Name,
                Description = category.Description
            };
            _appDbContext.Category.Add(newCategory);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            Category category = _appDbContext.Category.SingleOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(new CategoryUpdateVM { Name = category.Name , Description = category.Description});
        }

        [HttpPost]
        public IActionResult Edit(int id ,CategoryUpdateVM updateVM)
        {
            if (id == null) return NotFound();
            Category existCategory = _appDbContext.Category.Find(id);
            if (!ModelState.IsValid) return View();
            bool isExist = _appDbContext.Category.Any(c => c.Name.ToLower() == updateVM.Name.ToLower()&&c.Id!=id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "bu adli categori movcuddur");
                return View();
            }
            if (existCategory == null) return NotFound();
            existCategory.Description = updateVM.Description;
            existCategory.Name = updateVM.Name;
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Category category = _appDbContext.Category.SingleOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            _appDbContext.Category.Remove(category);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
