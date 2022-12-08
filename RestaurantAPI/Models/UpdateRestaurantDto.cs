using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class UpdateRestaurantDto
    {
        [Required]
        [MaxLength(24)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDelivery { get; set; }
    }
}
