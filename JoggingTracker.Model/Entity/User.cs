using System;
using System.Collections.Generic;
using JoggingTracker.Model.Enum;

namespace JoggingTracker.Model.Entity
{
    public class User : EntityBase
    {
        #region Property Lengths

        public const int UsernameLengthMin = 3;
        public const int UsernameLengthMax = 50;

        public const int EmailLengthMin = 5;
        public const int EmailLengthMax = 50;

        public const int PasswordLengthMin = 8;
        public const int PasswordLengthMax = 30;

        public const int NameLengthMin = 2;
        public const int NameLengthMax = 50;

        public const int SurnameLengthMin = 2;
        public const int SurnameLengthMax = 50;

        #endregion Property Lengths

        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<JoggingRoute> JoggingRoutes { get; set; }
    }
}
