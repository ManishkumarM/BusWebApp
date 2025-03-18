using BusWebApp.DTO;
using BusWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusWebApp.Controllers
{
    [Route("order")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService= orderService;
        }
        [HttpPost("{orderId}")]
       public async Task<IActionResult> UpdateOrderDetails([FromBody] OrderDto orderDto,string orderId)
        {
            try
            {
            //public int OrderId { get; set; }
            //public int UserId { get; set; }
            //public BusDto Bus { get; set; }
            //public int seatId { get; set; }
            //public List<TicketDto> Ticket { get; set; }

                if(orderDto== null)
                {
                    throw new Exception("Order Data cannot be empty or Invalid");
                }
                string message= await _orderService.AddOrder(orderDto);
                //await _ptService.AddTicket();
                return Ok("Order Added Succesfully "+ message);
            }
            catch (Exception ex)
            {
                throw new Exception( "Error "+ ex.Message);
            }
        }
    }
}
