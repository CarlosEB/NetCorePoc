using Microsoft.EntityFrameworkCore;
using NetCorePoc.Domain.Entities;

namespace NetCorePoc.Infrastructure.CrossCutting.DataAccess.Context
{
    public class DomainContext : DbContext
    {
        public DomainContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users{ get; set; }

    }
}
