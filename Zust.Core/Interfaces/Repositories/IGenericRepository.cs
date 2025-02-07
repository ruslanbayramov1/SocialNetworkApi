using System.Linq.Expressions;
using Zust.Core.Entities.Common;

namespace Zust.Core.Interfaces.Repositories;

public interface IGenericRepository<T> where T : BaseEntity, new()
{
    Task<List<U>> GetAllAsync<U>(Expression<Func<T, U>> select);
    Task<List<T>> GetAllAsync(Expression<Func<T, T>> select);
    Task<List<T>> GetAllAsync();

    Task<List<U>> GetWhereAsync<U>(Expression<Func<T, bool>> expression, Expression<Func<T, U>> select);
    Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> expression, Expression<Func<T, T>> select);
    Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> expression);


    Task<U?> GetByIdAsync<U>(Guid id, Expression<Func<T, U>> select);
    Task<T?> GetByIdAsync(Guid id, Expression<Func<T, T>> select);
    Task<T?> GetByIdAsync(Guid id);

    Task<U?> GetByExpressionAsync<U>(Expression<Func<T, bool>> expression, Expression<Func<T, U>> select);
    Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> expression, Expression<Func<T, T>> select);
    Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> expression);


    Task<bool> IsExistsAsync(Guid id);
    Task<bool> IsExistsAsync(Expression<Func<T, bool>> expression);
    Task AddAsync(T entity);
    Task RemoveAsync(Guid id);
    void Remove(T entity);
    void Update(T entity);
    Task<int> GetAllCountAsync();
    Task SaveAsync();
}
