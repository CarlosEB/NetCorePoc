using NetCorePoc.Application.DTOs;
using NetCorePoc.Domain.Entities;

namespace NetCorePoc.Application.Mapper
{
    public class AutoMapperApp
    {
        public static void ConfigureAutoMapper()
        {
            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<User, UserResponse>();
                cfg.CreateMap<UserRequest, User>();
            });
        }
    }
}