using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UniversityAccounting.DAL.Interfaces
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public interface IRepository<TEntity> where TEntity : class
    {
        int TotalCount();

        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetPart(int pageIndex, int pageSize);

        IEnumerable<TEntity> GetPart(string sortProperty, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending);

        IEnumerable<TEntity> GetPart(Expression<Func<TEntity, bool>> predicate, string sortProperty, int pageIndex,
            int pageSize, SortOrder sortOrder = SortOrder.Ascending);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
