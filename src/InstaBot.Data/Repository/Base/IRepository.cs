using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById<TKey>(TKey id);
        IEnumerable<TEntity> Query<TKey>(Expression<System.Func<TEntity, bool>> predicate);
        void Save(TEntity entity);
        void Save(IEnumerable<TEntity> entities);
        void Refresh(TEntity entity);
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);  
    }
}
