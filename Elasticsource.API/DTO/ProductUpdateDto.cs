using Elasticsource.API.Models;

namespace Elasticsource.API.DTO
{
    public record ProductUpdateDto(string Id, string name, decimal price, int stock, ProductFeatureCreateDto feature)
    {
    }
}
