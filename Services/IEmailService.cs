namespace BusWebApp.Services
{
    public interface IEmailService
    {
        public Task<string> SendEmail(string email, string subject);
    }
}
