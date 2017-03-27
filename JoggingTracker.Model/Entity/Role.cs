using System.Collections.Generic;

namespace JoggingTracker.Model.Entity
{
    public class Role : EntityBase
    {
        #region Property Lengths

        public const int NameLengthMax = 30;

        #endregion Property Lengths

        public const string AdminRole = "Admin";
        public const string UserManagerRole = "UserManager";
        public const string UserRole = "User";

        public static readonly string[] AllRoles;

        static Role()
        {
            Role.AllRoles = new string[]
            {
                Role.AdminRole,
                Role.UserManagerRole,
                Role.UserRole
            };
        }

        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
