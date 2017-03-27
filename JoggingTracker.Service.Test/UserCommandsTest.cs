using System;
using System.Threading.Tasks;
using AutoMapper;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Infrastructure.Provider.Security;
using JoggingTracker.Infrastructure.Provider.Time;
using JoggingTracker.Model.Entity;
using JoggingTracker.Model.Enum;
using JoggingTracker.Service.Implementation.Users;
using JoggingTracker.Service.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JoggingTracker.Service.Test
{
    public class UserCommandsTest : TestBase
    {
        private readonly ISecurityProvider _securityProvider;
        private readonly IServerTime _serverTime;
        private readonly IMapper _mapper;

        public UserCommandsTest() : base()
        {
            this._securityProvider = new SecurityProvider(1);
            this._serverTime = new ServerTime();
            this._mapper = new Mapper(new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            }));
        }

        [Fact]
        public async Task RegisterUserCommand_UserRegisters_NewUserCreated()
        {
            var command = new RegisterUserCommand()
            {
                Username = "testuser",
                Password = "testpassword123",
                ConfirmPassword = "testpassword123",
                Name = "Testname",
                Surname = "Testsurname",
                BirthDate = DateTime.Now,
                Email = "testemail@test.com",
                Gender = Gender.Male
            };

            UpdateUserCommand user;
            using (var dbContext = new JoggingTrackerDbContext(this._options))
            {
                dbContext.Roles.Add(new Role() { Id = Guid.NewGuid(), Name = Role.UserRole });
                await dbContext.SaveChangesAsync();

                var commandHandler = new RegisterUserCommandHandler(this._securityProvider, this._serverTime, this._mapper, dbContext);
                user = await commandHandler.Handle(command);
            }

            using (var dbContext = new JoggingTrackerDbContext(this._options))
            {
                Assert.NotNull(await dbContext.Users.SingleAsync(u => u.Id == user.Id));
            }
        }
    }
}
