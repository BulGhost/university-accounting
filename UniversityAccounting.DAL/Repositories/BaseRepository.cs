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

        public IEnumerable<TEntity> GetPart(string ordering, int pageIndex, int pageSize)
        {
            var type = typeof(TEntity);
            var prop = type.GetProperty(ordering);
            if (prop == null) throw new ArgumentException("Invalid property for ordering", ordering);

            var param = Expression.Parameter(type);
            var expr = Expression.Lambda<Func<TEntity, object>>(
                Expression.Convert(Expression.Property(param, prop), typeof(object)), param);

            return Context.Set<TEntity>()
                .OrderBy(expr)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();
        }

        public IEnumerable<TEntity> GetPart(Expression<Func<TEntity, bool>> predicate, string ordering, int pageIndex, int pageSize)
        {
            var type = typeof(TEntity);
            var prop = type.GetProperty(ordering);
            if (prop == null) throw new ArgumentException("Invalid property for ordering", ordering);

            var param = Expression.Parameter(type);
            var expr = Expression.Lambda<Func<TEntity, object>>(
                Expression.Convert(Expression.Property(param, prop), typeof(object)), param);

            return Context.Set<TEntity>()
                .Where(predicate)
                .OrderBy(expr)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();
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
    }
}
