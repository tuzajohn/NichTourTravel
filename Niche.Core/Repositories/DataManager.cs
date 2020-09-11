using Niche.Core.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Niche.Core.Repositories
{
    public class DataManager<TEntity> : IDataManager<TEntity> where TEntity : class
    {
        private readonly DbContextFactory _factory;
        public DataManager(DbContextFactory factory)
        {
            _factory = factory;
        }
        public TEntity Add(TEntity entity)
        {
            using var context = _factory.GetContext();
            context.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public void Delete(TEntity entity)
        {
            using var context = _factory.GetContext();
            context.Remove(entity);
            context.SaveChanges();
        }

        public TEntity Get(int id)
        {
            using var context = _factory.GetContext();
            var TEntity = context.Set<TEntity>().Find(id);
            return TEntity;
        }

        public IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> expression)
        {
            using var context = _factory.GetContext();
            var TEntities = context.Set<TEntity>().Where(expression).ToList();
            return TEntities;
        }

        public void Update(TEntity entity)
        {
            using var context = _factory.GetContext();
            context.Set<TEntity>().Update(entity);
            context.SaveChanges();
        }
    }
}
