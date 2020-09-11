using Niche.Core.IServices;
using Niche.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Niche.Services
{
    public class GlobalService<T> : IGlobalService<T> where T : class
    {
        private readonly IDataManager<T> _repo;
        public GlobalService(IDataManager<T> repo)
        {
            _repo = repo;
        }
        public T Add(T TEntity)
        {
            return _repo.Add(TEntity);
        }

        public void Delete(T TEntity)
        {
            _repo.Delete(TEntity);
        }

        public T Get(int id)
        {
            return _repo.Get(id);
        }

        public IEnumerable<T> Search(Expression<Func<T, bool>> expression)
        {
            return _repo.Search(expression);
        }

        public void Update(T TEntity)
        {
            _repo.Update(TEntity);
        }
    }
}
