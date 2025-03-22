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
	public class WeekdayRepository : Repository<Weekday>, IWeekdayRepository
	{
		private ApplicationDbContext _db;
		public WeekdayRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Weekday obj)
		{
			_db.Weekdays.Update(obj);
		}
	}
}
