using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusWebApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; } 

        [Required]
        [MinLength(6)]
        [JsonIgnore]
        public string? Password { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        public string Role { get; set; }

        public bool Active { get; set; }

        //REFERENCES
        public ICollection<Seat>? BookedSeats { get; set; }
        public ICollection<Seat>? LockedSeats { get; set; }
    }
}



