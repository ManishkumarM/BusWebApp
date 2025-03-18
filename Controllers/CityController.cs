using Microsoft.AspNetCore.Mvc;
using BusWebApp.Context;
using BusWebApp.Models;
using BusWebApp.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BusWebApp.Controllers
{
    //[Route("api/v{version:apiVersion}/[controller]/")]
    //[ApiVersion("1.2")]
    [ApiController]
    
    [Route("api/[controller]")]
    
    public class CityController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;
        public CityController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult<List<City>> GetAllCities()
        {
            try
            {
                List<City> data = _context.City.ToList();
                return data;
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :{ex.Message}");
            }
        }

        [HttpPost("search")]
        public async Task<ActionResult<List<City>>> GetCities([FromBody] SourceDestinationDto requestParam)
        {
            try
            {
                if (requestParam == null)
                    throw new Exception("Invalid RequestBody");

                string search = "";
                if (requestParam.Destination?.Trim() != null)
                    search = requestParam.Destination.Trim();
                if (search.Equals("") && requestParam.Source?.Trim() != null)
                    search = requestParam.Source.Trim();
                if (search.Equals(""))
                    search = "Invalid String";

                if (search.Equals("Invalid String"))
                    throw new Exception("Invalid RequestBody");

                search = search.ToUpper();
                Console.WriteLine("Search:::" + search);
                var filteredCities = await _context.City.Where(city => city.Name.ToUpper().Equals(search)).ToListAsync();

                return Ok(filteredCities);




            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception :::" + ex);
                return StatusCode(500, $"Internal Server Error :{ex.Message}");
            }
        }


        [HttpPost("showcity")]
        public IActionResult CheckSourceAndDestination([FromBody] SourceDestinationDto requestParam)
        {
            try
            {
                string source = (requestParam.Source ?? " ").Trim();
                string destination = (requestParam.Destination ?? " ").Trim();

                if (source == null || destination == null)
                    throw new Exception("Input values can't be null");
                 if (source.Equals("") || destination.Equals(""))
                    throw new Exception("Invalid Parameters source: "+source+" destination: "+destination);
                
                source= char.ToUpper(source[0])+ source.Substring(1);
                destination = char.ToUpper(destination[0]) + destination.Substring(1);

                City? fetchSourceData = _context.City.FirstOrDefault(city => (city.Name).Equals(source));
                City? fetchDestinationeData = _context.City.FirstOrDefault(city => (city.Name).Equals(destination));

                if (fetchSourceData != null)
                {
                    if (fetchDestinationeData != null)
                        return Ok(new { status = "success", data = "Buses in Your City are Here" });
                    else
                        return Ok(new { status = "failed" , data = "destination is not found"});
                }
                else
                {
                    throw new Exception("source is not found");
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
        
    }
}


