using System;
using System.Collections.Generic;

namespace InstaBot.Data.Repository
{
    public abstract class AbstractRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public abstract TEntity GetById<TKey>(TKey id);
        public abstract IEnumerable<TEntity> Query<TKey>(Func<TEntity, bool> predicate);
        public abstract void Save(IEnumerable<TEntity> entities);
        public abstract void Save(TEntity entity);
        public abstract void Delete(IEnumerable<TEntity> entities);
        public abstract void Delete(TEntity entity);
        public abstract void Refresh(TEntity entity);
    }
}