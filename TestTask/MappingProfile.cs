using AutoMapper;
using TestTask.Models;
using TestTask.Shared;

namespace TestTask
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FileEmail, FileEmailDto>();
            CreateMap<FileEmailDto, FileEmail>();
        }

    }
}
