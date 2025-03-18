using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusWebApp.Models
{
    public class Ticket
    {
        [Key]
        public string TicketId { get; set; }
        public string PassengerId { get; set; }
        [ForeignKey(nameof(PassengerId))]
        public Passenger Passenger { get; set; }
        public int SeatId { get; set; }
        public int BusId { get; set; }
        public string SourceLocation { get; set; }
        public string DestinationLocation { get; set; }
        public DateTime TravelDate { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public double FareAmount { get; set; }
    }
}
