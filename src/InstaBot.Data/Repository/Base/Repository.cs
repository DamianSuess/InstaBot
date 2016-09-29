using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ServiceStack.OrmLite;

namespace InstaBot.Data.Repository
{
    public class Repository<TEntity> : AbstractRepository<TEntity>, IDisposable where TEntity : class
    {
        private readonly IDbConnection _session;

        public Repository(IDbConnection session)
        {
            _session = session;
        }

        public override void Delete(IEnumerable<TEntity> entities)
        {
            _session.Delete<TEntity>(entities.ToArray());
        }

        public override void Save(IEnumerable<TEntity> entities)
        {
            _session.Save<TEntity>(entities.ToArray());
        }

        public override void Save(TEntity entity)
        {
            _session.Save<TEntity>(entity);
        }

        public override void Delete(TEntity entity)
        {
            _session.Delete<TEntity>(entity);
        }

        public override IEnumerable<TEntity> Query<TKey>(Func<TEntity, bool> predicate)
        {
            return _session.Select<TEntity>().Where(predicate).ToList();
        }

        public override TEntity GetById<TKey>(TKey id)
        {
            return _session.SingleById<TEntity>(id);
        }

        public override void Refresh(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if(_session != null)
                _session.Dispose();
        }
    }
}