using BusWebApp.Context;
using BusWebApp.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusWebApp.Controllers
{
    public class SeatController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SeatController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{busId}")]
        public async Task<IActionResult> GetSeatsByBus(int busId)
        {
            try
            {
                var seats = await _context.Seat
                    .Where(s => s.BusId == busId)
                    .Select(s => new
                    {
                        s.SeatId,
                        s.LockedUntil,
                        s.LockedBy,
                        s.BookedBy,
                    })
                    .ToListAsync();

                return Ok(seats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while fetching seats.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("lock")]
        public async Task<IActionResult> LockSeat([FromBody] SeatRequest request)
        {
            try
            {
                var seat = await _context.Seat.Where(s => s.SeatId == request.SeatId && s.BusId == request.BusId).FirstOrDefaultAsync();

                if (seat == null || seat.BookedBy != null)
                {
                    return BadRequest("Seat is already booked or does not exist.");
                }

                if (seat.LockedUntil != null && seat.LockedUntil > DateTime.UtcNow)
                {
                    return BadRequest("Seat is already locked by another user.");
                }

                seat.LockedBy = request.UserId;
                seat.LockedUntil = DateTime.UtcNow.AddMinutes(3);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Seat locked for 3 minutes" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while locking the seat.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookSeat([FromBody] SeatRequest request)
        {

            try
            {
                var seat = await _context.Seat.Where(s => s.SeatId == request.SeatId && s.BusId == request.BusId).FirstOrDefaultAsync();

                if (seat == null || seat.BookedBy != null)
                {
                    return BadRequest("Seat is already booked or does not exist.");
                }

                if ((seat.LockedBy!=null && seat.LockedBy != request.UserId )|| (seat.LockedUntil!= null && seat.LockedUntil < DateTime.UtcNow))
                {
                    return BadRequest("Seat lock expired or not locked by this user.");
                }

                seat.BookedBy = request.UserId;
                seat.LockedBy = null;
                seat.LockedUntil = null;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Seat booked successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while booking the seat.", error = ex.Message });
            }
        }

    }
}
