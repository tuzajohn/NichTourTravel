using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Niche.Core.IServices
{
    public interface IGlobalService<TEntity> where TEntity: class
    {
        TEntity Add(TEntity TEntity);
        void Update(TEntity TEntity);
        TEntity Get(int id);
        void Delete(TEntity TEntity);
        IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> expression);
    }
}
