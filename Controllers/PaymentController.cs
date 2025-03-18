using BusWebApp.Context;
using BusWebApp.DTO;
using BusWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace BusWebApp.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 1️⃣ Create Razorpay Order
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] PaymentRequestDto request)
        {
            try
            {
                var keyId = _configuration["Razorpay:KeyId"];
                var keySecret = _configuration["Razorpay:KeySecret"];

                var client = new RazorpayClient(keyId, keySecret);

                var options = new Dictionary<string, object>
                {
                    { "amount", request.Amount * 100 }, // Amount in paise
                    { "currency", "INR" },
                    { "receipt", Guid.NewGuid().ToString() }
                };

                Order order = client.Order.Create(options);
                string orderId = order["id"].ToString();

                // Save order details in DB
                var payment = new CustomPayment
                {
                    OrderId = orderId,
                    Amount = request.Amount,
                    Status = "Pending"
                };

                await _context.CustomPayment.AddAsync(payment);
                await _context.SaveChangesAsync();

                return Ok(new { orderId, keyId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 2️⃣ Verify Razorpay Payment
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] PaymentVerificationDto request)
        {
            try
            {
                var keySecret = _configuration["Razorpay:KeySecret"];
                string generatedSignature;

                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(keySecret)))
                {
                    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.OrderId + "|" + request.PaymentId));
                    generatedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();
                }

                if (generatedSignature == request.Signature)
                {
                    // Update Payment Status in DB
                    var payment = await _context.CustomPayment.FirstOrDefaultAsync(p => p.OrderId == request.OrderId);
                    if (payment != null)
                    {
                        payment.PaymentId = request.PaymentId;
                        payment.Signature = request.Signature;
                        payment.Status = "Paid";

                        _context.CustomPayment.Update(payment);
                        await _context.SaveChangesAsync();
                    }
                    //Call to PassengerService for Adding Passengers and Generate TicketIds.. for the Passengers.

                    return Ok(new { message = "Payment verified successfully" });
                }

                return BadRequest(new { message = "Invalid signature" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
