using System.Security.Cryptography.X509Certificates;
using BusWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BusWebApp.Context

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Bus> Bus { get; set; }
        public DbSet<Seat> Seat { get; set; }
        //public DbSet<Order> Order { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<City> City { get; set; }
        //public DbSet<TicketSummary> TicketSummary  { get; set; }
        public DbSet<CustomPayment> CustomPayment { get; set; }

        public DbSet<Ticket> Ticket{ get; set; }
        public DbSet<Passenger> Passenger { get; set; }

        public DbSet<CustomOrder> CustomOrder { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seat>()
               .HasOne(s => s.BookedByUser)
               .WithMany(u => u.BookedSeats)
               .HasForeignKey(s => s.BookedBy)
               .OnDelete(DeleteBehavior.Restrict); // Prevents cascading delete

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.LockedByUser)
                .WithMany(u => u.LockedSeats)
                .HasForeignKey(s => s.LockedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
