using System;
using AutoMapper;
using WebApi.Data;
using WebApi.DTO;

namespace WebApi.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Students, StudentDTO>();
        CreateMap<StudentDTO, Students>();

    }
}
