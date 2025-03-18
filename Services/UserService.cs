

using System.Security.Cryptography;
using System.Text;
using BusWebApp.Context;
using BusWebApp.Helpers;
using BusWebApp.Models;
using Microsoft.EntityFrameworkCore;
namespace BusWebApp.Services
{

    public class UserService:IUserService
    {
        //private static int setterId = 1000;
        private static readonly object _lock = new object();
        //private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext   _context;
        private readonly JwtService _jwtService;
        private IEmailService _emailService;
        public UserService(ApplicationDbContext context, IEmailService emailService, JwtService jwtService)
        {
            //_appSettings = appSettings.Value;
            _emailService = emailService;
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<User?> AddAndUpdateUser(User userObj)
        {
            bool isSuccess = false;
            if (userObj.Id > 0)
            {
                var obj = await _context.User.FirstOrDefaultAsync(user=> user.Id == userObj.Id);
                if(obj != null)
                {
                    obj.Name = userObj.Name;
                    obj.Email = userObj.Email;
                    _context.User.Update(obj);
                    isSuccess = await _context.SaveChangesAsync()>0;
                }
            }
            else
            {
                userObj.Id= generateId();
                userObj.Password= HashPassword(userObj.Password);
                userObj.Role = "Customer";
                await _context.User.AddAsync(userObj);
                isSuccess= await _context.SaveChangesAsync() >0;
                string message= await _emailService.SendEmail(userObj.Email, "User and Email Registered Succesfully");
                Console.WriteLine("From UserService:: "+message);

            }

            return isSuccess ? userObj : null;
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Name== model.Username);
            if (user == null)
                return null;
            if (!VerifyPassword(model.Password, user.Password))
                return null;
            var token =  _jwtService.GenerateToken(user);
            return new AuthenticateResponse(user, token);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.User.Where(user => user.Active== true).ToListAsync();
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.User.FirstOrDefaultAsync(user=> user.Id== id);
        }

        

        public int generateId()
        {
            lock(_lock){
                return BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return HashPassword(inputPassword) == hashedPassword;
        }

    }
}
