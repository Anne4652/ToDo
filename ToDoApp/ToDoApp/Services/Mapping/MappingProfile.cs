using AutoMapper;
using ToDoApp.DataAccess.Entities;
using ToDoApp.Services.DTOs;

namespace ToDoApp.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DataAccess.Entities.Task, TaskDto>();
            CreateMap<TaskDto, DataAccess.Entities.Task>();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<DataAccess.Entities.Task, CreateTaskDto>().ReverseMap();
        }
    }
}
