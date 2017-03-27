using AutoMapper;
using JoggingTracker.Model.Entity;
using JoggingTracker.Service.Implementation.JoggingRoutes;
using JoggingTracker.Service.Implementation.Users;

namespace JoggingTracker.Service.Infrastructure.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<User, UpdateUserCommand>();
            this.CreateMap<JoggingRoute, UpdateJoggingRouteCommand>();
        }
    }
}
