using NetCorePoc.Domain.Entities;
using NetCorePoc.Domain.Interfaces.Repositories;
using NetCorePoc.Infrastructure.CrossCutting.DataAccess.Context;

namespace NetCorePoc.Infrastructure.CrossCutting.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User, int>, IUserRepository
    {
        public UserRepository(DomainContext context) : base(context)
        {
        }
    }
}
