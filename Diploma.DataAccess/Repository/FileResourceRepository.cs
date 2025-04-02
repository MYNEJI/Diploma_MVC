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
	public class FileResourceRepository : Repository<FileResource>, IFileResourceRepository
	{
		private ApplicationDbContext _db;
		public FileResourceRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FileResource obj)
		{
			_db.FileResources.Update(obj);
		}
	}
}
