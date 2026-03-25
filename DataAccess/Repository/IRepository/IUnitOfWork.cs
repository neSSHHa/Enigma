using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{

		IApplicationUserRepository ApplicationUser { get; }
		ILectureRepository Lecture { get; }
		IPopRepository Pop { get; }
		Task SaveAsyncI();

		
	}
}
