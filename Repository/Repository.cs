using _0sechill.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace _0sechill.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected ApiDbContext context;

        public Repository(ApiDbContext context)
        {
            this.context = context;
        }
        public IQueryable<T> FindAll() => context.Set<T>().AsNoTracking();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            context.Set<T>().Where(expression).AsNoTracking();
        public void Create(T entity) => context.Set<T>().Add(entity);

        public void Update(T entity) => context.Set<T>().Update(entity);
        public void Delete(T entity) => context.Set<T>().Remove(entity);
        public void AttachEntity(T entity) => context.Attach<T>(entity);

        public void DeleteRange(IEnumerable<T> entities) => context.Set<T>().RemoveRange(entities);
    }
}
