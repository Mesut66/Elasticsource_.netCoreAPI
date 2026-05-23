using System.ComponentModel.DataAnnotations;

namespace Elasticsource.API.Models
{
    public enum EColor
    {
        [Display(Name = "Kırmızı")]
        Red = 1,
        [Display(Name = "Yeşil")]
        Green = 2,
        [Display(Name = "Mavi")]
        Blue = 3,
        [Display(Name = "Sarı")]
        Yellow = 4,
        [Display(Name = "Siyah")]
        Black = 5,
        [Display(Name = "Beyaz")]
        White = 6
    }
}
