using BusWebApp.DTO;

namespace BusWebApp.Services
{
    public interface IOrderService
    {
        public Task<string?> AddOrder(OrderDto orderDto);
    }
}
