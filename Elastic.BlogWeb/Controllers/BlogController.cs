using Elastic.BlogWeb.Models;
using Elastic.BlogWeb.Services;
using Elastic.BlogWeb.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Elastic.BlogWeb.Controllers
{
    public class BlogController : Controller
    {

        private BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        public IActionResult Search()
        {
            return View(new List<Blog>());
        }
        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            var blogList = await _blogService.SearchAsync(searchText);
            return View(blogList);
        }
        public IActionResult Save()
        {
            return View();
        }

        public async Task<IActionResult> SaveBlog(BlogCreateVM blog)
        {
            if (!ModelState.IsValid) return View("Save", blog);
            var result = await _blogService.CreateBlogAsync(blog);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Blog kaydedilirken bir hata oluştu.";
                return View("Save", blog);
            }

            TempData["SuccessMessage"] = "Blog başarıyla kaydedildi.";
            return View("Save");

        }
    }
}
