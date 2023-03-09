using FrontToBack.DAL;
using FrontToBack.Extensions;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
      
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_appDbContext.Sliders.ToList());
        }
        public IActionResult Create()
        { 
            return View();
        }
        [HttpPost]
        public IActionResult Create(SliderCreateVM sliderCreateVM) 
        {
            if (sliderCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please Input Image");
                return View();
                
            }
            if(!sliderCreateVM.Photo.IsImage()) 
            {
                ModelState.AddModelError("Photo", "Please input only Image");
                return View();
            }
            if (sliderCreateVM.Photo.CheckSize(500))
            {
                ModelState.AddModelError("Photo", "Photo size is Large");
                return View();
            }

            Slider newSlider = new();
            newSlider.ImageUrl = sliderCreateVM.Photo.SaveImage(_env,"img",sliderCreateVM.Photo.FileName);

            _appDbContext.Sliders.Add(newSlider);
            _appDbContext.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id) 
        {
            if (id == 0) return NotFound();
            var slider = _appDbContext.Sliders.SingleOrDefault(s=> s.Id == id);
            if (slider == null) return NotFound();

            string fullPath = Path.Combine(_env.WebRootPath,"img",slider.ImageUrl);
            if (System.IO.File.Exists(fullPath))
            {

              System.IO.File.Delete(fullPath);

            }

            _appDbContext.Sliders.Remove(slider);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id) 
        {
            if (id == null) return NotFound();
            Slider slider = _appDbContext.Sliders.SingleOrDefault(c => c.Id == id);
            if (slider == null) return NotFound();
            return View(new SliderUpdateVM { ImageUrl = slider.ImageUrl});
        }

        [HttpPost]
        public IActionResult Edit(int id,SliderUpdateVM updateVM) 
        {
            if (id == null) return NotFound();
            Slider slider = _appDbContext.Sliders.SingleOrDefault(c => c.Id == id);
            if (slider == null) return NotFound();
            if (updateVM.Photo!=null)
            {
                if (!updateVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please input only Image");
                    return View();
                }
                if (updateVM.Photo.CheckSize(500))
                {
                    ModelState.AddModelError("Photo", "Photo size is Large");
                    return View();
                }


                string fullPath = Path.Combine(_env.WebRootPath, "img", slider.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {

                    System.IO.File.Delete(fullPath);

                }
               
                slider.ImageUrl = updateVM.Photo.SaveImage(_env,"img",updateVM.Photo.FileName);
                _appDbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }

   
}
