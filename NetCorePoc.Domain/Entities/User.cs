using NetCorePoc.Domain.Entities.Bases;

namespace NetCorePoc.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
