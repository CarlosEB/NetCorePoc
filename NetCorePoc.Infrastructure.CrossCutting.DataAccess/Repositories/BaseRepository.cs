using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NetCorePoc.Domain.Entities.Bases;
using NetCorePoc.Domain.Interfaces.Repositories;
using NetCorePoc.Infrastructure.CrossCutting.DataAccess.Context;

namespace NetCorePoc.Infrastructure.CrossCutting.DataAccess.Repositories
{
    public abstract class BaseRepository<TEntity, TPrimaryKey> : IBaseRepository<TEntity, TPrimaryKey> where TEntity : BaseEntity<TPrimaryKey>
    {
        public DomainContext Context { get; set; }        

        protected BaseRepository(DomainContext context)
        {
            Context = context;
        }


        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public virtual void Create(TEntity obj)
        {
            Context.Set<TEntity>().Add(obj);
        }

        public TEntity Details(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual void Edit(TEntity obj)
        {
            Context.Entry(obj).State = EntityState.Modified;
        }

        public IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> exp)
        {
            return Context.Set<TEntity>().Where(exp).ToList();
        }

        public int GetTotal()
        {
            return Context.Set<TEntity>().Count();
        }

        public int GetTotalWhere(Expression<Func<TEntity, bool>> exp)
        {
            return GetWhere(exp).Count();
        }

        public virtual void RemoveWhere(Expression<Func<TEntity, bool>> exp)
        {
            GetWhere(exp)
                .ToList()
                .ForEach(i => Context.Set<TEntity>().Remove(i));
        }
    }
}