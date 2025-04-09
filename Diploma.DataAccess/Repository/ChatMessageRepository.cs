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
	public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
	{
		private ApplicationDbContext _db;
		public ChatMessageRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(ChatMessage obj)
		{
			_db.ChatMessages.Update(obj);
		}
	}
}
