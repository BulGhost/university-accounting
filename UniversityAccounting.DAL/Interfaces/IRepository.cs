using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UniversityAccounting.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int TotalCount();

        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetPart(int pageIndex, int pageSize);
        IEnumerable<TEntity> GetPart(string ordering, int pageIndex, int pageSize);

        IEnumerable<TEntity> GetPart(Expression<Func<TEntity, bool>> predicate, string ordering, int pageIndex,
            int pageSize);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
