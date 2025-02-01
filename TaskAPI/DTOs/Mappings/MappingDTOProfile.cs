using AutoMapper;
using TaskAPI.DTOs.UserDTO;
using TaskAPI.Models;

namespace TaskAPI.DTOs.Mappings
{
    public class MappingDTOProfile : Profile
    {
        public MappingDTOProfile() 
        { 
            CreateMap<UserModel, RegisterDTO>().ReverseMap();
        }
    }
}
