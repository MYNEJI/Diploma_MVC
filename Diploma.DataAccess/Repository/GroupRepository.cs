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
	public class GroupRepository : Repository<Group>, IGroupRepository
	{
		private ApplicationDbContext _db;
		public GroupRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Group obj)
		{
			_db.Groups.Update(obj);
		}
	}
}
