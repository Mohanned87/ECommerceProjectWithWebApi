using Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Concrete
{
    public class EfBaseRepository<TEntity, TContex> : IBaseRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContex : DbContext, new()
    {
        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContex contex = new TContex())
            {
                return filter == null ? await contex.Set<TEntity>().ToListAsync() : await contex.Set<TEntity>().Where(filter).ToListAsync();
            }
        }
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            using (TContex contex = new TContex())
            {
                return await contex.Set<TEntity>().SingleOrDefaultAsync(filter);
            }
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using (TContex contex = new TContex())
            {
                await contex.Set<TEntity>().AddAsync(entity);
                await contex.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using (TContex contex = new TContex())
            {
                contex.Set<TEntity>().Update(entity);
                await contex.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (TContex contex = new TContex())
            {
                var deleteEntity = await contex.Set<TEntity>().FindAsync(id);
                contex.Set<TEntity>().Remove(deleteEntity);
                var data =await contex.SaveChangesAsync();
                if (data > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
