using Elasticsource.API.Models;
using Nest;

namespace Elasticsource.API.DTO
{
    public record ProductDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto? Feature)
    {

    }
}
