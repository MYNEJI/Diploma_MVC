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
	public class GroupWeekdayRepository : Repository<GroupWeekday>, IGroupWeekdayRepository
	{
		private ApplicationDbContext _db;
		public GroupWeekdayRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(GroupWeekday obj)
		{
			_db.GroupWeekdays.Update(obj);
		}
	}
}
