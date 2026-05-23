using Elasticsource.API.Models;

namespace Elasticsource.API.DTO
{
    public record ProductFeatureCreateDto(int? Width, int? Height, EColor? Color)
    {
    }
}
