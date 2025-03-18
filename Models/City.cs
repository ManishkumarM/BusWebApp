using System.ComponentModel.DataAnnotations;

namespace BusWebApp.Models
{
    public class City
    {
        [Key]
        public required string CityId { get; set; } // Primary Key

        [Required]
        [MaxLength(100)] // Adjust length based on requirements
        public required string? Name { get; set; } 

        [Required]
        [MaxLength(100)] // Adjust length based on requirements
        public required string? State { get; set; }
    }
}
