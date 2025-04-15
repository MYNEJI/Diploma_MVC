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
	public class LessonRepository : Repository<Lesson>, ILessonRepository
	{
		private ApplicationDbContext _db;
		public LessonRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Lesson obj)
		{
			_db.Lessons.Update(obj);
		}
	}
}
