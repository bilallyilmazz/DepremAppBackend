using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, new()
    {

        Task<T> Get(Expression<Func<T, bool>> filter= null);
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null);
        Task<int> Add(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(T entity);
    }
}
