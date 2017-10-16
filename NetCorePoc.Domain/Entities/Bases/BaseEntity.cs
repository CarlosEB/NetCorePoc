using System;
using NetCorePoc.Domain.Interfaces.Bases;

namespace NetCorePoc.Domain.Entities.Bases
{
    public abstract class BaseEntity<TPrimaryKey> : IBaseEntity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
        public DateTime CreatedAt { get ; set; }
        public DateTime? UpdatedAt { get; set; }        
    }

    public abstract class BaseEntity : BaseEntity<int>, IBaseEntity
    {
    }
}
