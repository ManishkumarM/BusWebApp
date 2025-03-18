using System.Numerics;
using AutoMapper;
using BusWebApp.Context;
using BusWebApp.DTO;
using BusWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]/")]
    //[ApiVersion("1.2")]
    //[Authorize(Roles ="Admin,Customer")]
    public class BusController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BusController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: HomeController
        [HttpGet("GetAllFromJson")]
        public Task<IActionResult> GetAllBusesFromJson()
        {
            //string relativePath = Path.Combine("Data", "buses.json");
            //string projectRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
            //string filePath = Path.Combine(projectRoot, relativePath);
            string fileFolder = Directory.GetCurrentDirectory() + "\\Data\\buses.json";
            string filePath = fileFolder;
            Console.WriteLine("File:::" + filePath);
            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return Task.FromResult<IActionResult>(NotFound(new { message = "JSON file not found." }));
            }

            // Read the JSON file
            string jsonString = System.IO.File.ReadAllText(filePath);


            // Return JSON content
            return Task.FromResult<IActionResult>(Content(jsonString, "application/json"));
        }

        [HttpPost("addNew")]
        public async Task<IActionResult> AddBus(BusDto busDto)
        {
            try
            {
                if (busDto == null)
                {
                    throw new Exception("bus object is null");
                }
                var bus = _mapper.Map<Bus>(busDto);
                await _context.Bus.AddAsync(bus);
                await _context.SaveChangesAsync();

                // Create 30 seats for the new bus
                var seats = new List<Seat>();
                for (int i = 1; i <= 30; i++)
                {
                    seats.Add(new Seat
                    {
                        SeatId = (busDto.Id * 100) + i,  // Assuming auto-increment
                        BusId = busDto.Id,
     
                    });
                }

                await _context.Seat.AddRangeAsync(seats);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Bus created with 30 seats.", bus.BookedSeats });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("getAll")]
        public IActionResult GetAllBuses([FromBody] SourceDestinationDto requestBody)
        {
            try
            {
                string source = (requestBody.Source ?? " ").Trim();
                string destination = (requestBody.Destination ?? " ").Trim();

                if (source == null || destination == null)
                    throw new Exception("Input values can't be null");
                if (source.Equals("") || destination.Equals(""))
                    throw new Exception("Invalid Parameters source: " + source + " destination: " + destination);

                source = char.ToUpper(source[0]) + source.Substring(1);
                destination = char.ToUpper(destination[0]) + destination.Substring(1);

                Bus? busData = _context.Bus.FirstOrDefault(bus => bus.From.Equals(source) && bus.To.Equals(destination));
                if (busData == null)
                    return NotFound($"Bus not found with the details: {source} and {destination}");
                BusDto busDtodata = _mapper.Map<BusDto>(busData);
                return Ok(busData);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Message: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BusDto>> getOne(string id)
        {
            try
            {
                //var Id = int.Parse(id);
                Bus? bus = await _context.Bus.FindAsync(int.Parse(id));
                if (bus == null)
                    return NotFound($"Bus Data not found with {id}");
                BusDto busDtodata = _mapper.Map<BusDto>(bus);
                return Ok(busDtodata);

            }
            catch (Exception exp)
            {
                throw new Exception("Exception Occured " + exp);
            }
        }


        [HttpDelete("id")]
        public async Task<IActionResult> DeleteBus(string id)
        {
            try
            {
                Bus? bus = await _context.Bus.FindAsync(int.Parse(id));
                if (bus == null)
                    throw new Exception("Bus not found with provided Id: " + id);
                int deletedRowCount = await _context.Bus.Where(bus => bus.Id == int.Parse(id)).ExecuteDeleteAsync();
                if (deletedRowCount > 0)
                    return Ok("Successfully Deleted Bus with Id " + id);
                else
                    return Ok("No Bus got Deleted .Please try it later");
            }
            catch (Exception ex)
            {
                throw new Exception("Exception occured: " + ex.Message);
            }
        }
    }
}
