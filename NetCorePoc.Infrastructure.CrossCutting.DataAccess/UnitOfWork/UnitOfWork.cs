using NetCorePoc.Domain.Interfaces.UnitOfWork;
using NetCorePoc.Infrastructure.CrossCutting.DataAccess.Context;

namespace NetCorePoc.Infrastructure.CrossCutting.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DomainContext _context;
        public UnitOfWork(DomainContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
