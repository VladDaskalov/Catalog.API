using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs
{
    public class CreateItemDTO
    {
        [Required]
        public string Name { get; init; }
        [Required]
        [Range(1, 1000)]
        public decimal Price { get; init; }
    }
}