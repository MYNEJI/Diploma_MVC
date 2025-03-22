using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.DataAccess.Repository.IRepository
{
	public interface IWeekdayRepository : IRepository<Weekday>
	{
		void Update(Weekday obj);
	}
}
