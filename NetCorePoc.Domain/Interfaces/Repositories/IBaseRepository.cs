using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NetCorePoc.Domain.Interfaces.Bases;

namespace NetCorePoc.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity, TPrimaryKey> where TEntity : IBaseEntity<TPrimaryKey>
    {
        IEnumerable<TEntity> GetAll();
        void Create(TEntity obj);
        TEntity Details(int id);
        void Edit(TEntity obj);
        IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> exp);
        int GetTotal();
        int GetTotalWhere(Expression<Func<TEntity, bool>> exp);

        void RemoveWhere(Expression<Func<TEntity, bool>> exp);
    }

    public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, int> where TEntity : IBaseEntity<int>
    {
        
    }
}
