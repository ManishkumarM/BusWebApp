using System.ComponentModel.DataAnnotations;
namespace BusWebApp.Models
{
    public class Bus
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [MaxLength(100)] // Adjust length based on requirements
        public string? CompanyName { get; set; }

        [MaxLength(50)] // Adjust length based on requirements
        public string? From { get; set; } 

        [MaxLength(50)] // Adjust length based on requirements
        public string? To { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public int NoOfSeats { get; set; } = 30;

        public List<string> Amenities { get; set; } = [];// Equivalent to an array of strings

        [Range(0, 5)]
        public double? Rating { get; set; }

        [MaxLength(10)] // Adjust based on expected time format
        public string? Arrival { get; set; } 

        [MaxLength(10)] // Adjust based on expected time format
        public string? Departure { get; set; }
        //REFERENCES
        public ICollection<Seat>? BookedSeats { get; set; }
    }
}
