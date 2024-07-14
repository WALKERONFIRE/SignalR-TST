using AutoMapper;
using SignalR_TST.DTOs;
using SignalR_TST.Models;

namespace SignalR_TST
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
        
    }
}
