using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UniversityAccounting.DAL.Interfaces;

namespace UniversityAccounting.DAL.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public BaseRepository(DbContext context)
        {
            Context = context;
        }

        public int TotalCount()
        {
            return Context.Set<TEntity>().Count();
        }

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsEnumerable();
        }

        public IEnumerable<TEntity> GetPart(int pageIndex, int pageSize)
        {
            return Context.Set<TEntity>()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();
        }

        public IEnumerable<TEntity> GetPart(string sortProperty, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending)
        {
            var expr = GetKeySelector(typeof(TEntity), sortProperty);

            var items = sortOrder == SortOrder.Ascending
                ? Context.Set<TEntity>().OrderBy(expr)
                : Context.Set<TEntity>().OrderByDescending(expr);

            return items
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<TEntity> GetPart(Expression<Func<TEntity, bool>> predicate, string sortProperty,
            int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            var expr = GetKeySelector(typeof(TEntity), sortProperty);

            var items = sortOrder == SortOrder.Ascending
                ? Context.Set<TEntity>().Where(predicate).OrderBy(expr)
                : Context.Set<TEntity>().Where(predicate).OrderByDescending(expr);

            return items
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        private Expression<Func<TEntity, object>> GetKeySelector(Type type, string sortProperty)
        {
            var prop = type.GetProperty(sortProperty);
            if (prop == null) throw new ArgumentException("Invalid property for ordering", sortProperty);

            var param = Expression.Parameter(type);
            return Expression.Lambda<Func<TEntity, object>>(
                Expression.Convert(Expression.Property(param, prop), typeof(object)), param);
        }
    }
}
