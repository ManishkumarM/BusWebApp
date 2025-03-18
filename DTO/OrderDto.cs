using BusWebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusWebApp.DTO
{
    public class OrderDto
    {
        [Required(ErrorMessage = "OrderId is required.")]
        public int OrderId {  get; set; }
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        
        
        [Required(ErrorMessage = "Ticket is required.")]
        public List<TicketDto> Tickets { get; set; }

    }
    public class TicketDto
    {
        [Required(ErrorMessage = "Passenger is required.")]
        public PassengerDto Passenger { get; set; }
        [Required(ErrorMessage = "Bus is required.")]
        public BusDto Bus { get; set; }
        [Required(ErrorMessage = "SeatId is required.")]
        public int seatId { get; set; }
        [Required(ErrorMessage = "FareAmount is required.")]
        public double FareAmount { get; set; }
        [Required(ErrorMessage = "TravelDate is required.")]
        public DateTime TravelDate { get; set; }
    }
}