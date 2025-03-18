using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusWebApp.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }
        
        public DateTime? LockedUntil { get; set; }  // Lock expiration time
        
        public int? BusId { get; set; }
        //[ForeignKey(nameof(BusId))]
        //public Bus? Bus { get; set; }  // Navigation property to the Bus

        public int? BookedBy { get; set; }
        [ForeignKey(nameof(BookedBy))]
        public User? BookedByUser { get; set; }  // Navigation property to the Bus

        public int? LockedBy { get; set; }
        [ForeignKey(nameof(LockedBy))]
        public User? LockedByUser { get; set; }  // Navigation property to the Bus

        public string? PassengerId { get; set; }
        [ForeignKey(nameof(PassengerId))]
        public Passenger Passenger { get; set; }
    }
}
