using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusWebApp.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiVersion("2.0")] // Specify the version of this controller
    public class ProductController : ControllerBase
    {
        //Get All Products
        [HttpGet("getAll")]
        public IActionResult GetProducts()
        {
            var products = new[]
            {
                new { mobile = "Samsung", Price = "12000" },
                new { mobile = "Tv", Price = "320000" }
            };
           

            return Ok(products);
        }
    }
}
