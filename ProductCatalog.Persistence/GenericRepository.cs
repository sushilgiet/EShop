using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Persistance
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        DbSet<T> dbSet;
        public GenericRepository(DbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public async virtual Task<T> Add(T item)
        {
            dbSet.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async virtual Task Delete(int id)
        {
            T entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync<T>();
        }

        public async virtual Task<T> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async virtual Task Update(T item)
        {
            _context.Entry<T>(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
