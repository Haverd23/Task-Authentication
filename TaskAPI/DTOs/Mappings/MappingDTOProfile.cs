using AutoMapper;
using TaskAPI.DTOs.RefreshTokenDTO;
using TaskAPI.DTOs.TaskDTO;
using TaskAPI.DTOs.UserDTO;
using TaskAPI.Models;

namespace TaskAPI.DTOs.Mappings
{
    public class MappingDTOProfile : Profile
    {
        public MappingDTOProfile() 
        { 
            CreateMap<UserModel, RegisterDTO>().ReverseMap();
            CreateMap<UserModel, LoginDTO>().ReverseMap();
            CreateMap<TaskModel, CreateTaskDTO>().ReverseMap();
            CreateMap<TaskModel, UpdateTaskDTO>().ReverseMap();
            CreateMap<RefreshTokenModel, RefreshTokenRequestDTO>().ReverseMap();
        }
    }
}
