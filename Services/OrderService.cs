
using BusWebApp.Context;
using BusWebApp.DTO;
using BusWebApp.Models;

namespace BusWebApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string?> AddOrder(OrderDto orderDto)
        {
            try
            {
                CustomOrder order = new CustomOrder();
                order.OrderId = orderDto.OrderId;
                order.UserId = orderDto.UserId;
                order.ConfirmedAt = DateTime.UtcNow;
                List<Ticket> tickets = new List<Ticket>();
                List<Passenger> passengers = new List<Passenger>();

                foreach (var ticketDto in orderDto.Tickets)
                {
                    Passenger passenger = new Passenger();
                    passenger.Id = "PSG-" + Guid.NewGuid().ToString("N").Substring(0, 10);
                    passenger.AadharId = ticketDto.Passenger.AadharId;
                    passenger.Email = ticketDto.Passenger.Email;
                    passenger.Gender = ticketDto.Passenger.Gender;
                    passenger.Name = ticketDto.Passenger.Name;
                    passenger.Phone= ticketDto.Passenger.Phone;
                    passengers.Add(passenger);

                    Ticket ticket = new Ticket();
                    ticket.TicketId = "TKT-" + Guid.NewGuid().ToString("N").Substring(0, 10);
                    Console.WriteLine("Ticket::: "+ ticket.TicketId);
                    ticket.SeatId = ticketDto.seatId;
                    ticket.FareAmount = ticketDto.FareAmount;
                    ticket.BusId = ticketDto.Bus.Id;
                    ticket.ArrivalTime = ticketDto.Bus.Arrival;
                    ticket.DepartureTime = ticketDto.Bus.Departure;
                    ticket.SourceLocation = ticketDto.Bus.From;
                    ticket.DestinationLocation = ticketDto.Bus.To;
                    ticket.TravelDate = ticketDto.TravelDate.ToUniversalTime();
                    ticket.PassengerId = passenger.Id;
                    tickets.Add(ticket);

                }

                await _context.Passenger.AddRangeAsync(passengers);
                await _context.Ticket.AddRangeAsync(tickets);
                await _context.CustomOrder.AddAsync(order);
                await _context.SaveChangesAsync();

                return "Order got updated successfully ...!";
            }
            catch(Exception ex)
            {
                throw new Exception(ex?.StackTrace);
            }
        }
    }
}
