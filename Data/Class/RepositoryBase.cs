using _0sechill.Data.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace _0sechill.Data.Class
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApiDbContext context;

        public RepositoryBase(ApiDbContext context)
        {
            this.context = context;
        }
        public IQueryable<T> FindAll() => context.Set<T>().AsNoTracking();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            context.Set<T>().Where(expression).AsNoTracking();
        public void Create(T entity) => context.Set<T>().Add(entity);
        public void Update(T entity) => context.Set<T>().Update(entity);
        public void Delete(T entity) => context.Set<T>().Remove(entity);
    }
}
