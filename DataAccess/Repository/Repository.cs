using DataAccess.DataManipulation;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		internal DbSet<T> _set;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
			_set = _context.Set<T>();
        }

        public async Task addAsync(T item)
		{
			await _set.AddAsync(item);
		}

		public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> patern, string? included)
		{

			var query = _set.Where(patern);
            if (included != null)
            {
                foreach (var includedItem in included.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includedItem);
                }
            }
            return await query.FirstOrDefaultAsync();
		}

		public IQueryable<T> GetAll(Expression<Func<T, bool>>? patern, string? included)
		{
            IQueryable<T> query = _set;
            if (included != null)
            {
                foreach (var includedItem in included.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includedItem);
                }
            }
            if (patern != null)
				return query.Where(patern);

			return query;
		}

		public void Remove(T item)
		{
			_set.Remove(item);
		}
	}
}
