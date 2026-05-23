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
