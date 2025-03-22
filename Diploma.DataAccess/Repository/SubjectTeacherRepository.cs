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
	public class SubjectTeacherRepository : Repository<SubjectTeacher>, ISubjectTeacherRepository
	{
		private ApplicationDbContext _db;
		public SubjectTeacherRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(SubjectTeacher obj)
		{
			_db.SubjectTeachers.Update(obj);
		}
	}
}
