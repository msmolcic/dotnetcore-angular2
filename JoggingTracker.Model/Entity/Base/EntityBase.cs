using System;
using System.ComponentModel.DataAnnotations;

namespace JoggingTracker.Model.Entity
{
    /// <summary>
    /// Base class of every database entity.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Database entity primary key.
        /// </summary>
        [Key]
        public Guid Id { get; set; }
    }
}
