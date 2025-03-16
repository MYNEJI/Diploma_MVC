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
	public class UnitOfWork : IUnitOfWork
	{
		private ApplicationDbContext _db;
		public ICategoryRepository Category { get; private set; }
		public ICompanyRepository Company { get; private set; }
		public ISubjectRepository Subject { get; private set; }
		public IShoppingCartRepository ShoppingCart { get; private set; }
		public ICourseEnrollmentRequestRepository CourseEnrollmentRequest { get; private set; }
		public IApplicationUserRepository ApplicationUser { get; private set; }
		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			ApplicationUser = new ApplicationUserRepository(_db);
			ShoppingCart = new ShoppingCartRepository(_db);
			CourseEnrollmentRequest = new CourseEnrollmentRequestRepository(_db);
			Category = new CategoryRepository(_db);
			Subject = new SubjectRepository(_db);
			Company = new CompanyRepository(_db);
		}		

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
