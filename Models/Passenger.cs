using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusWebApp.Models;


namespace BusWebApp.Models
{    public class Passenger
    {
        public string Id { get; set; }
        public string AadharId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
    }
}
