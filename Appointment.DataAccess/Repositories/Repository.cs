using Appointment.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Appointment.DataAccess.Context;

namespace Appointment.DataAccess.Repositories
{
    public class Repository<T>(AppointmentDbContext dbContext) : IRepository<T> where T : BaseEntity, new()
    {
        private DbSet<T> Table => dbContext.Set<T>();

        public async Task AddAsync(T entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task AddListAsync(List<T> entity)
        {
            await Table.AddRangeAsync(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await Table.AsNoTracking().Where(r => !r.IsDeleted).AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null ? await Table.AsNoTracking().Where(r => !r.IsDeleted).CountAsync() : await Table.AsNoTracking().Where(r => !r.IsDeleted).CountAsync(predicate);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = false,
            int currentPage = 1, int pageSize = 100)
        {
            IQueryable<T> queryable = Table.Where(p => !p.IsDeleted);
            if (!enableTracking) queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);
            if (predicate is not null) queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            return await queryable.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<List<TRes>> GetAllAsync<TRes>(Expression<Func<T, TRes>> select,
            Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>,
                IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = true,
            int currentPage = 1, int pageSize = 100)
        {
            IQueryable<T> queryable = Table.Where(p => !p.IsDeleted);
            if (!enableTracking) queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);
            if (predicate is not null) queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).Select(select).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            return await queryable.Select(select).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<List<T>> GetAllNoPaginationAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = true)
        {
            IQueryable<T> queryable = Table.Where(p => !p.IsDeleted);
            if (!enableTracking) queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);
            if (predicate is not null) queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).ToListAsync();
            return await queryable.ToListAsync();
        }

        public async Task<List<TRes>> GetAllNoPaginationAsync<TRes>(
            Expression<Func<T, TRes>> select,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = true)
        {
            IQueryable<T> queryable = Table.Where(p => !p.IsDeleted);
            if (!enableTracking) queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);
            if (predicate is not null) queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).Select(select).ToListAsync();
            return await queryable.Select(select).ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool enableTracking = false)
        {
            IQueryable<T> queryable = Table.Where(r => !r.IsDeleted);
            if (!enableTracking) queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);
            return await queryable.FirstOrDefaultAsync(predicate);
        }

        public async Task<TRes?> GetAsync<TRes>(Expression<Func<T, TRes>> select,
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool enableTracking = true)
        {
            IQueryable<T> queryable = Table.Where(r => !r.IsDeleted);
            if (!enableTracking) queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);
            return await queryable.Where(predicate).Select(select).FirstOrDefaultAsync();
        }

    }
}

