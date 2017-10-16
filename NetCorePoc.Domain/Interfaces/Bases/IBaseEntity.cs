using System;

namespace NetCorePoc.Domain.Interfaces.Bases
{
    public interface IBaseEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime? UpdatedAt { get; set; }
    }

    public interface IBaseEntity : IBaseEntity<int>
    {
    }
}
