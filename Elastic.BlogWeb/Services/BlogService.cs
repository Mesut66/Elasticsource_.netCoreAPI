using Elastic.BlogWeb.Models;
using Elastic.BlogWeb.Repository;
using Elastic.BlogWeb.ViewModel;

namespace Elastic.BlogWeb.Services
{
    public class BlogService
    {
        private readonly BlogRepository _repository;

        public BlogService(BlogRepository repository)
        {
            _repository = repository;
        }
        public async Task<Blog?> CreateBlogAsync(BlogCreateVM blog)
        {
            return await _repository.SaveAsync(new Blog
            {
                Title = blog.Title,
                Content = blog.Content,
                Tags = blog.Tags.ToArray(),
                UserId = Guid.NewGuid()
            });
        }
    }
}
