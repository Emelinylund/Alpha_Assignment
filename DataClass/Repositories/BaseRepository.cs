﻿using DataClass.Contexts;
using DataClass.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DataClass.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<RepositoryResult<bool>> AddAsync(TEntity entity);
    Task<RepositoryResult<bool>> DeleteAsync(TEntity entity);
    Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> findBy);
    Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync();
    Task<RepositoryResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> findBy);
    Task<RepositoryResult<bool>> UpdateAsync(TEntity entity);
}


public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _table;

    protected BaseRepository(AppDbContext context)
    {
        _context = context;
        _table = context.Set<TEntity>();
    }

    public virtual async Task<RepositoryResult<bool>> AddAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Add(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync()
    {
        var entities = await _table.ToListAsync();
        return new RepositoryResult<IEnumerable<TEntity>> { Succeeded = true, StatusCode = 200, Result = entities };
    }

    public virtual async Task<RepositoryResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> findBy)
    {
        var entity = await _table.FirstOrDefaultAsync(findBy);
        return entity == null
            ? new RepositoryResult<TEntity> { Succeeded = false, StatusCode = 404, Error = "Entity not found." }
            : new RepositoryResult<TEntity> { Succeeded = true, StatusCode = 200, Result = entity };
    }


    public virtual async Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> findBy)
    {
        var exists = await _table.AnyAsync(findBy);
        return !exists
             ? new RepositoryResult<bool> { Succeeded = false, StatusCode = 404, Error = "Entity not found." }
             : new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
    }

    public virtual async Task<RepositoryResult<bool>> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Update(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepositoryResult<bool>> DeleteAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

   
}
