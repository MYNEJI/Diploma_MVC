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
	public class ClassroomRepository : Repository<Classroom>, IClassroomRepository
	{
		private ApplicationDbContext _db;
		public ClassroomRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Classroom obj)
		{
			_db.Classrooms.Update(obj);
		}
	}
}
