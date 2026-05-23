using Elasticsource.API.Models;

namespace Elasticsource.API.DTO
{
    public record ProductDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto? Feature)
    {

    }
}
