using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusWebApp.Models;

namespace BusWebApp.Models
{
    public class CustomOrder
    {
        [Key]
        public int OrderId { get; set; } // Primary key for the order

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime ConfirmedAt { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

       ICollection<Passenger> Tickets { get; set; }

    }

}
