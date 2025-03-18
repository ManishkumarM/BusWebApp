namespace BusWebApp.DTO
{
    public class PaymentRequestDto
    {
        public decimal Amount { get; set; }
    }

    public class PaymentVerificationDto
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; }
    }
}
