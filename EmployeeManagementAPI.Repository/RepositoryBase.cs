using EmployeeManagementAPI.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeManagementAPI.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext repositoryContext;

        protected RepositoryBase(RepositoryContext context) =>
            repositoryContext = context;

        public void Create(T entity) =>
            repositoryContext.Set<T>().Add(entity);

        public void Update(T entity) =>
            repositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) =>
            repositoryContext.Set<T>().Remove(entity);

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
            repositoryContext.Set<T>()
            .AsNoTracking()
            : repositoryContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? repositoryContext.Set<T>()
            .Where(expression)
            .AsNoTracking()
            : repositoryContext.Set<T>()
            .Where(expression);
    }
}
