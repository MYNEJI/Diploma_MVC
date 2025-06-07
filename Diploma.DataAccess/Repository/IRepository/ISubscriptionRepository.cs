using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.DataAccess.Repository.IRepository
{
	public interface ISubscriptionRepository : IRepository<Subscription>
	{
		void Update(Subscription obj);
		void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);
	}
}
