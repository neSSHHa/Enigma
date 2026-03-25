using DataAccess.DataManipulation;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
	{

		private readonly ApplicationDbContext _db;
        public IApplicationUserRepository ApplicationUser { get; private set; }
		public ILectureRepository Lecture { get; private set; }
		public IPopRepository Pop { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            ApplicationUser = new ApplicationUserRepository(db);
			Lecture = new LectureRepository(db);
			Pop = new PopRepository(db);
            _db = db;
        }

		public async Task SaveAsyncI()
		{
			await _db.SaveChangesAsync();
		}
	}
}
