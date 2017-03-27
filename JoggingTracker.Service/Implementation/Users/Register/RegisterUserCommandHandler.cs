using System;
using System.Threading.Tasks;
using AutoMapper;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Infrastructure.Provider.Security;
using JoggingTracker.Infrastructure.Provider.Time;
using JoggingTracker.Model.Entity;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.Users
{
    public class RegisterUserCommandHandler : IAsyncRequestHandler<RegisterUserCommand, UpdateUserCommand>
    {
        private readonly ISecurityProvider _securityProvider;
        private readonly IServerTime _serverTime;
        private readonly IMapper _mapper;
        private readonly JoggingTrackerDbContext _dbContext;

        public RegisterUserCommandHandler(
            ISecurityProvider securityProvider,
            IServerTime serverTime,
            IMapper mapper,
            JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new
            {
                securityProvider,
                serverTime,
                mapper,
                dbContext
            });

            this._securityProvider = securityProvider;
            this._serverTime = serverTime;
            this._mapper = mapper;
            this._dbContext = dbContext;
        }

        public async Task<UpdateUserCommand> Handle(RegisterUserCommand command)
        {
            var role = await this._dbContext.Roles.SingleAsync(r => r.Name == Role.UserRole);
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = command.Username,
                Password = this._securityProvider.CalculatePasswordHash(command.Password),
                Email = command.Email,
                RegistrationDate = this._serverTime.UtcNow,
                Name = command.Name,
                Surname = command.Surname,
                BirthDate = command.BirthDate.Value,
                Gender = command.Gender.Value
            };

            this._dbContext.Users.Add(user);
            this._dbContext.UserRoles.Add(new UserRole()
            {
                RoleId = role.Id,
                UserId = user.Id
            });

            await this._dbContext.SaveChangesAsync();

            return this._mapper.Map<UpdateUserCommand>(user);
        }
    }
}
