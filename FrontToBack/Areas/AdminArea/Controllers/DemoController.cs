using FrontToBack.DAL;
using FrontToBack.Extensions;
using FrontToBack.Models;
using FrontToBack.Models.Demo;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrontToBack.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class DemoController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public DemoController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Authors = new SelectList(_appDbContext.Authors.ToList(),"Id","Name");
            ViewBag.Genres = new SelectList(_appDbContext.Genres.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookCreateVM bookCreateVM)
        {
            ViewBag.Authors = new SelectList(_appDbContext.Authors.ToList(), "Id", "Name");
            ViewBag.Genres = new SelectList(_appDbContext.Genres.ToList(), "Id", "Name");


            List<BookImages>bookImages= new();
            foreach (var photo in bookCreateVM.Photos)
            {
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please input only Image");
                    return View();
                }
                if (photo.CheckSize(500))
                {
                    ModelState.AddModelError("Photo", "Photo size is Large");
                    return View();
                }
                BookImages bookImage = new();
                bookImage.ImageUrl = photo.SaveImage(_env, "img", photo.FileName);
                bookImages.Add(bookImage);
            }


            List<BookGenre> bookGenres = new();
            List<BookAuthor> bookAuthors = new();
            Book newBook = new();
            foreach (var item in bookCreateVM.GenreIds)
            {
                BookGenre bookGenre = new();
                bookGenre.BookId = newBook.Id;
                bookGenre.GenreId = item;
                bookGenres.Add(bookGenre);
            }
            foreach (var item in bookCreateVM.AuthorIds)
            {
                BookAuthor bookAuthor = new();
                bookAuthor.BookId = newBook.Id;
                bookAuthor.AuthorId = item;
                bookAuthors.Add(bookAuthor);
            }
            newBook.Name = bookCreateVM.Name;
            newBook.BookGenres =bookGenres;
            newBook.BookAuthors =bookAuthors;
            newBook.BookImages =bookImages;

            _appDbContext.Books.Add(newBook);
            _appDbContext.SaveChanges();
            return View();
        }

    }
}
