using Elasticsource.API.Models;

namespace Elasticsource.API.DTO
{
    public record ProductCreateDto(string name, decimal price, int stock, ProductFeatureCreateDto feature)//record 9.0 la geldi .İmmutable(değişmez) yani değiştirilemez nesneler oluşturmak için kullanılır.Örneğin bir product oluşturduktan sonra özelliklerini değiştirmek istemiyorsak record kullanabiliriz.
    {
        public Product CreateProduct()
        {
            return new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Price = price,
                Stock = stock,
                Feature = new ProductFeature
                {

                    Width = (int)feature.Width,
                    Height = (int)feature.Height,
                    Color = feature.Color ?? EColor.Black
                }
            };
        }
    }
}
