using BusWebApp.Context;
using BusWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BusWebApp.Services
{
    public class SeatReleaseService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SeatReleaseService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var expiredSeats = await _context.Seat
                        .Where(s => s.LockedUntil != null && s.LockedUntil < DateTime.UtcNow)
                        .ToListAsync();

                    foreach(var seat in expiredSeats) {
                        seat.LockedBy = null;
                        seat.LockedUntil = null;
                    }

                    await _context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Check every 1 minute
            }
        }
    }

}
