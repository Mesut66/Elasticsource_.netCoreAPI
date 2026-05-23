using Elasticsource.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Elasticsource.API.DTO
{
    public record ProductFeatureDto(int? Width, int? Height, string? ColorName)
    {
    }
}
