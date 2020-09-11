using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Niche.Core.Repositories
{
    public interface IDataManager<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
        void Update(TEntity entity);
        TEntity Get(int id);
        void Delete(TEntity entity);
        IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> expression);
    }
}
