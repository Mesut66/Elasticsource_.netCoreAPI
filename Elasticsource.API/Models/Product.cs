using Elasticsource.API.DTO;
using Nest;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Elasticsource.API.Models
{
    public class Product
    {
        [PropertyName("_id")]//aşağıdaki id buna karşılık gelecek es yi haberdar ederiz ben id göndermezsen bunu otomatik olarak oluşturur ama ben id göndermek istiyorum o zaman bu şekilde yaparız
        public string Id { get; set; } = null!;//Bu ıd bizim asıl datalarda bulunmuyor
        public string Name { get; set; } = null!;//null olamaz demek için null! ekledik
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public ProductFeature? Feature { get; set; }

        private string? GetColorDisplayName(EColor? color)
        {
            if (color == null)
                return null;

            var field = color.Value.GetType().GetField(color.Value.ToString());
            var attribute = field?.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>();
            return attribute?.Name ?? color.Value.ToString();
        }

        public ProductDto CreateDto()
        {
            if (Feature == null)
                return new ProductDto(Id, Name, Price, Stock, null);

            return new ProductDto(Id, Name, Price, Stock, new ProductFeatureDto(Feature.Width, Feature.Height, GetColorDisplayName(Feature.Color)));
        }

    }
}
