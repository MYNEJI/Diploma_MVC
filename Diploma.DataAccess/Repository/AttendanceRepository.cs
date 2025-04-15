using Diploma.DataAccess.Data;
using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.DataAccess.Repository
{
	public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
	{
		private ApplicationDbContext _db;
		public AttendanceRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Attendance obj)
		{
			_db.Attendances.Update(obj);
		}
	}
}
