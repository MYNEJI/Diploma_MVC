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
	public class SubjectRepository : Repository<Subject>, ISubjectRepository
	{
		private ApplicationDbContext _db;
		public SubjectRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Subject obj)
		{
			var objFromDb = _db.Subjects.FirstOrDefault(u => u.Id == obj.Id);
			if (objFromDb != null)
			{
				objFromDb.Title = obj.Title;
				objFromDb.ISBN = obj.ISBN;
				objFromDb.Price = obj.Price;
				objFromDb.Price50 = obj.Price50;
				objFromDb.ListPrice = obj.ListPrice;
				objFromDb.Price100 = obj.Price100;
				objFromDb.Description = obj.Description;
				objFromDb.CategoryId = obj.CategoryId;
				if (obj.ImageUrl != null)
				{
					objFromDb.ImageUrl = obj.ImageUrl;
				}
			}
		}
	}
}
