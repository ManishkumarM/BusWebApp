using AutoMapper;
using BusWebApp.Models;
using BusWebApp.DTO;
namespace BusWebApp
{
    public class MappingProfile:Profile
    {
       public MappingProfile()
        {
            CreateMap<BusDto, Bus>();
            CreateMap<Bus, BusDto>();
            CreateMap<UserDto, User>();
        }
    }
}
