using System;
using System.Linq;
using JoggingTracker.Infrastructure.Provider.Security;
using JoggingTracker.Infrastructure.Provider.Time;
using JoggingTracker.Model.Entity;
using JoggingTracker.Model.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingTracker.DataAccess.Database
{
    public sealed class DatabaseInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider, bool isDevelopment)
        {
            var context = serviceProvider.GetService<JoggingTrackerDbContext>();
            context.Database.Migrate();

            DatabaseInitializer.InitializeApplicationRoles(context);

            if (!isDevelopment)
                return;

            var securityProvider = serviceProvider.GetService<ISecurityProvider>();
            var serverTime = serviceProvider.GetService<IServerTime>();

            DatabaseInitializer.InitializeAdministratorUser(context, securityProvider, serverTime);
            DatabaseInitializer.InitializeUserManager(context, securityProvider, serverTime);
            DatabaseInitializer.InitializeUsersWithRoutes(context, securityProvider, serverTime);
        }

        private static void InitializeApplicationRoles(JoggingTrackerDbContext context)
        {
            foreach (var roleName in Role.AllRoles)
            {
                if (context.Roles.Any(role => role.Name == roleName))
                    continue;

                context.Roles.Add(new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = roleName
                });
            }

            context.SaveChanges();
        }

        private static void InitializeAdministratorUser(
            JoggingTrackerDbContext context,
            ISecurityProvider securityProvider,
            IServerTime serverTime)
        {
            const string adminUsername = "admin";

            if (context.Users.Any(user => user.Username == adminUsername))
                return;

            var userId = Guid.NewGuid();
            var adminUser = new User()
            {
                Id = userId,
                Username = adminUsername,
                Password = securityProvider.CalculatePasswordHash("admin"),
                Email = "admin@jogging-tracker.com",
                RegistrationDate = serverTime.UtcNow,
                Name = "Admin",
                Surname = "Administratior",
                BirthDate = new DateTime(1992, 2, 5),
                Gender = Gender.Male
            };

            context.Users.Add(adminUser);
            context.UserRoles.Add(new UserRole()
            {
                Role = context.Roles.Single(role => role.Name == Role.AdminRole),
                User = adminUser
            });

            context.SaveChanges();
        }

        private static void InitializeUserManager(
            JoggingTrackerDbContext context,
            ISecurityProvider securityProvider,
            IServerTime serverTime)
        {
            const string userManagerUsername = "manager";

            if (context.Users.Any(user => user.Username == userManagerUsername))
                return;

            var userId = Guid.NewGuid();
            var managerUser = new User()
            {
                Id = userId,
                Username = userManagerUsername,
                Password = securityProvider.CalculatePasswordHash("manager"),
                Email = "manager@jogging-tracker.com",
                RegistrationDate = serverTime.UtcNow,
                Name = "John",
                Surname = "Doe",
                BirthDate = new DateTime(1992, 2, 5),
                Gender = Gender.Male
            };

            context.Users.Add(managerUser);
            context.UserRoles.Add(new UserRole()
            {
                Role = context.Roles.Single(role => role.Name == Role.UserManagerRole),
                User = managerUser
            });

            context.SaveChanges();
        }

        private static void InitializeUsersWithRoutes(
            JoggingTrackerDbContext context,
            ISecurityProvider securityProvider,
            IServerTime serverTime)
        {
            const string userUsernamePrefix = "User";

            if (context.Users.Any(user => user.Username.StartsWith(userUsernamePrefix)))
                return;

            var random = new Random();

            for (int userOrdinal = 1; userOrdinal <= 20; userOrdinal++)
            {
                var userUsername = $"{userUsernamePrefix}{userOrdinal}";
                var year = 1995 - userOrdinal;
                var month = userOrdinal % 12;
                month = month == 0 ? userOrdinal : month;
                var day = userOrdinal % 27;
                day = day == 0 ? userOrdinal : day;
                var gender = (Gender)random.Next(0, 1);

                var userId = Guid.NewGuid();
                var user = new User()
                {
                    Id = userId,
                    Username = userUsername,
                    Password = securityProvider.CalculatePasswordHash($"{123}{userUsername}"),
                    Email = $"{userUsername}@jogging-tracker.com",
                    RegistrationDate = serverTime.UtcNow,
                    Name = userUsername,
                    Surname = userUsername,
                    BirthDate = new DateTime(year, month, day),
                    Gender = gender
                };

                context.Users.Add(user);
                context.UserRoles.Add(new UserRole()
                {
                    Role = context.Roles.Single(role => role.Name == Role.UserRole),
                    User = user
                });

                var runningStartDate = new DateTime(serverTime.UtcNow.Year - 1, user.BirthDate.Month, user.BirthDate.Day);
                DateTime? previous = null;

                for (int routeOrdinal = 0; routeOrdinal < 15; routeOrdinal++)
                {
                    var distance = random.NextDouble() * 25;
                    distance = distance == 0 ? 1 : distance;

                    var hours = random.Next(0, 24);
                    var minutes = random.Next(0, 60);
                    var seconds = random.Next(0, 60);

                    if (previous == null)
                        previous = runningStartDate;
                    else
                        previous.Value.AddDays(random.Next(1, 3));

                    var startTime = previous.Value.Add(new TimeSpan(hours, minutes, seconds));
                    var endTime = startTime.AddMinutes(random.Next(15, 120));
                    endTime = endTime.AddSeconds(random.Next(0, 60));

                    var joggingRoute = new JoggingRoute()
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        DistanceKilometers = (decimal)distance,
                        StartTime = startTime,
                        EndTime = endTime,
                        Date = startTime.Date
                    };

                    context.JoggingRoutes.Add(joggingRoute);

                    previous = endTime;
                }
            }

            context.SaveChanges();
        }
    }
}
