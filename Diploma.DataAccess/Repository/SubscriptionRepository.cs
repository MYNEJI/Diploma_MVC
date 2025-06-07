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
	public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
	{
		private ApplicationDbContext _db;
		public SubscriptionRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Subscription obj)
		{
			_db.Subscriptions.Update(obj);
		}

		public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
		{
			var orderFromDb = _db.Subscriptions.FirstOrDefault(o => o.Id == id);
			if (!string.IsNullOrEmpty(sessionId))
			{
				orderFromDb.SessionId = sessionId;
			}
			if (!string.IsNullOrEmpty(paymentIntentId))
			{
				orderFromDb.PaymentDate = DateTime.Now;
			}
		}
	}
}
