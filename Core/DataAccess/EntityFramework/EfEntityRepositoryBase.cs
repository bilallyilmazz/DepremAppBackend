using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
	public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
		where TEntity : class, new()

	{
		private readonly DbContext _context;

		public EfEntityRepositoryBase(DbContext context)
		{
			_context = context;
		}

		public async Task<int> Add(TEntity entity)
		{
			
				var addedEntity = _context.Entry(entity);
				addedEntity.State = EntityState.Added;
				return await _context.SaveChangesAsync();

		}

		public async Task<int> Delete(TEntity entity)
		{
			
				var addedEntity = _context.Entry(entity);
				addedEntity.State = EntityState.Deleted;
				return await _context.SaveChangesAsync();
			

		}

		public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
		{
		
				IQueryable<TEntity> query = _context.Set<TEntity>();

				var result = await query.AsNoTracking().FirstOrDefaultAsync(filter);

				return result;
			
		}

		public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null)
		{
		
				IQueryable<TEntity> query = _context.Set<TEntity>();

				if (filter != null)
				{
					query = query.Where(filter);
				}

				var result = await query.AsNoTracking().ToListAsync();

				return result;
			
		}

		public async Task<int> Update(TEntity entity)
		{
			
				var addedEntity = _context.Entry(entity);
				addedEntity.State = EntityState.Modified;
				return await _context.SaveChangesAsync();
			
		}
	}
}
