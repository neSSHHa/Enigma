using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> patern, string? included=null);
		IQueryable<T> GetAll(Expression<Func<T, bool>>? patern = null, string? included=null);
		Task addAsync(T item);
		void Remove(T item);
	}
}
