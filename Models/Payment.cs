using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusWebApp.Models;

namespace BusWebApp.Models
{
    public class CustomPayment
    {
        [Key]
        public int Id { get; set; }

        public string OrderId { get; set; }
        public string? PaymentId { get; set; }
        public string? Signature { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "INR";
        public string Status { get; set; } = "Pending"; // Pending, Paid, Failed
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
