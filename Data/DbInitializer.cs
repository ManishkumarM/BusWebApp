using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using BusWebApp.Context;
using BusWebApp.Models;  // Import your models
using Microsoft.EntityFrameworkCore;

namespace BusWebApp.Data // Change namespace as per your project structure
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure the database is created and apply migrations
            context.Database.Migrate();

            if (!context.City.Any()) // Prevent duplicate inserts
            {
                var jsonData = File.ReadAllText("./Data/cities.json"); // Read JSON
                var jsonArray = JsonNode.Parse(jsonData)?.AsArray(); // Parse as JSON Array
                if (jsonArray != null)
                {
                    var cities = jsonArray.Select(city => new City
                    {
                        CityId = city["_id"]?["$oid"]?.ToString() ?? Guid.NewGuid().ToString(), // Extract MongoDB ID
                        Name = city["name"]?.ToString(),
                        State = city["state"]?.ToString()
                    }).ToList();

                    context.City.AddRange(cities);
                    context.SaveChanges();
                }
            }
            //using var sha= SHA256.Create();
            if (!context.User.Any())
            {
                User user1 = new User();
                user1.Id = 1;
                user1.Name = "Admin";
                user1.Active = true;
                user1.Email = "admin@gmail.com";
                user1.Role = "Admin";
                user1.Password = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("admin")));

                User user2 = new User();
                user2.Id = 2;
                user2.Name = "Manish";
                user2.Active = true;
                user2.Email = "manish@gmail.com";
                user2.Role = "Customer";
                user2.Password = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("manish")));

                User user3 = new User();
                user3.Id = 3;
                user3.Name = "Manager";
                user3.Active = true;
                user3.Email = "manager@gmail.com";
                user3.Role = "Manager";
                user3.Password = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("manager")));
                
                context.User.Add(user1);
                context.User.Add(user2);
                context.User.Add(user3);
                context.SaveChanges();
            }
        }
    }
}
