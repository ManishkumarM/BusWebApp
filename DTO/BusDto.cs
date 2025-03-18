namespace BusWebApp.DTO
{
    public class BusDto
    {
    
        public int Id { get; set; } 
        public string? CompanyName { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public decimal Price { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public List<string> Amenities { get; set; } = [];// Equivalent to an array of strings
        public double? Rating { get; set; }
        public string? Arrival { get; set; }
        public string? Departure { get; set; }

    }
}
