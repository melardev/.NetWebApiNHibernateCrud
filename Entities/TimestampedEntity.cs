using System;

namespace WebApiNHibernateCrud.Entities
{
    public abstract class TimestampedEntity 
    {
        public virtual DateTime? CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        
    }
}