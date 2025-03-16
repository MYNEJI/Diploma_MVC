using Diploma.DataAccess.Data;
using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.DataAccess.Repository
{
	public class CourseEnrollmentRequestRepository : Repository<CourseEnrollmentRequest>, ICourseEnrollmentRequestRepository
	{
		private ApplicationDbContext _db;
		public CourseEnrollmentRequestRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(CourseEnrollmentRequest obj)
		{
			_db.CourseEnrollmentRequests.Update(obj);
		}
	}
}
