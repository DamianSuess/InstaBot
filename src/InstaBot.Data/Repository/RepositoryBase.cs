using System.Collections.Generic;
using ServiceStack;

namespace InstaBot.Data.Repository
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public abstract void Delete(IEnumerable<TEntity> entities);

        public abstract void Delete(TEntity entity);

        public abstract TEntity GetById<TKey>(TKey id);

        public abstract void Refresh(TEntity entity);

        public abstract void Save(IEnumerable<TEntity> entities);

        public abstract void Save(TEntity entity);
    }
}