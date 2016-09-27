using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById<TKey>(TKey id);
        void Refresh(TEntity entity);
        void Save(TEntity entity);
        void Save(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);  
    }
}
