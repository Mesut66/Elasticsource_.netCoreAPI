using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elastic.BlogWeb.ViewModel
{
    public class BlogCreateVM
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
        public string  Tags { get; set; } = null!;

    }
}
