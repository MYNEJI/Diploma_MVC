using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ICategoryRepository Category { get; }
		ISubjectRepository Subject { get; }
		ICompanyRepository Company { get; }
		IShoppingCartRepository ShoppingCart { get; }
		ICourseEnrollmentRequestRepository CourseEnrollmentRequest { get; }
		IApplicationUserRepository ApplicationUser { get; }
		void Save();
	}
}
