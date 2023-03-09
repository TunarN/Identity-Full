using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FrontToBack.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BasketController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Add(int id , string name)
        {
            if (id==null)
            {
                return NotFound();
            }
            Product product = await _appDbContext.Products.FindAsync(id);
             
            if (product==null)
            {
                return NotFound();
            }
            List<BasketVM> products;
            if (Request.Cookies["basket"]==null)
            {
                products = new();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            BasketVM existproduct = products.FirstOrDefault(p => p.Id == id);
            if (existproduct==null)
            {
                BasketVM basketVM = new();
                basketVM.Id = product.Id;
                basketVM.BasketCount = 1;
                products.Add(basketVM);
               
            }

            else
            {
                existproduct.BasketCount++;
            }
            
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromHours(1) });
            return RedirectToAction(nameof(Index),"Home");
        }
        public IActionResult ShowBasket()
        {
            List<BasketVM> products;
            string basket = Request.Cookies["basket"];
            if (basket==null)
            {
                products = new();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var item in products)
                {
                    Product currentProduct = _appDbContext.Products.Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id ==item.Id); ;
                    item.Name = currentProduct.Name;
                    item.Price = currentProduct.Price;
                    item.Id = currentProduct.Id;
                    item.ImageUrl = currentProduct.ProductImages.FirstOrDefault().ImageUrl;
                }
            }
        
            return View(products);
        }
        public async Task<IActionResult> DeleteItem(int id)
        {
            List<BasketVM> products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            products.Remove(products.FirstOrDefault(p => p.Id == id));
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromHours(1) });
            return RedirectToAction(nameof(ShowBasket));
        }
    }
}
