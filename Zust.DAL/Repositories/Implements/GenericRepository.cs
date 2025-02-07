using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Zust.Core.Entities.Common;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
{
    private readonly AppDbContext _context;
    protected DbSet<T> Table;
    public GenericRepository(AppDbContext context)
    {
        _context = context;
        Table = context.Set<T>();
    }

    public async Task AddAsync(T entity)
        => await Table.AddAsync(entity);

    public async Task<List<U>> GetAllAsync<U>(Expression<Func<T, U>> select)
    {
        var entities = await Table.Select(select).ToListAsync();
        return entities;
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, T>> select)
    {
        var entities = await Table.Select(select).ToListAsync();
        return entities;
    }

    public async Task<List<T>> GetAllAsync()
    {
        var entities = await Table.ToListAsync();
        return entities;
    }

    public async Task<U?> GetByExpressionAsync<U>(Expression<Func<T, bool>> expression, Expression<Func<T, U>> select)
    {
        var query = Table.Where(expression).Select(select);
        var entity = await query.FirstOrDefaultAsync();
        return entity;
    }

    public async Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> expression, Expression<Func<T, T>> select)
    {
        var query = Table.Where(expression).Select(select);
        var entity = await query.FirstOrDefaultAsync();
        return entity;
    }

    public async Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> expression)
    {
        var entity = await Table.FirstOrDefaultAsync(expression);
        return entity;
    }

    public async Task<U?> GetByIdAsync<U>(Guid id, Expression<Func<T, U>> select)
        => await GetByExpressionAsync(x => x.Id == id, select);

    public async Task<T?> GetByIdAsync(Guid id, Expression<Func<T, T>> select)
    => await GetByExpressionAsync(x => x.Id == id, select);

    public async Task<T?> GetByIdAsync(Guid id)
    => await Table.FindAsync(id);

    public async Task<List<U>> GetWhereAsync<U>(Expression<Func<T, bool>> expression, Expression<Func<T, U>> select)
    {
        var query = Table.Where(expression).Select(select);
        var entities = await query.ToListAsync();
        return entities;
    }

    public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> expression, Expression<Func<T, T>> select)
    {
        var query = Table.Where(expression).Select(select);
        var entities = await query.ToListAsync();
        return entities;
    }

    public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> expression)
    {
        var query = Table.Where(expression);
        var entities = await query.ToListAsync();
        return entities;
    }

    public async Task<bool> IsExistsAsync(Guid id)
    {
        var entity = await Table.FindAsync(id);
        if (entity == null) return false;
        return true;
    }

    public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> expression)
    {
        var entity = await Table.FirstOrDefaultAsync(expression);
        if (entity == null) return false;
        return true;
    }

    public async Task RemoveAsync(Guid id)
    {
        var entity = await Table.FindAsync(id);
        Table.Remove(entity!);
    }

    public async Task<int> GetAllCountAsync()
        => await Table.CountAsync();

    public void Remove(T entity)
        => Table.Remove(entity);

    public void Update(T entity)
        => Table.Update(entity);

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}
