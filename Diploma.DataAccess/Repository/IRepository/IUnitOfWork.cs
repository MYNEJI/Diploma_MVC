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
		IWeekdayRepository Weekday { get; }
		IGroupRepository Group { get; }
		IGroupStudentRepository GroupStudent { get; }
		IGroupWeekdayRepository GroupWeekday { get; }
		ISubjectTeacherRepository SubjectTeacher { get; }
		IFileResourceRepository FileResource { get; }
		IChatMessageRepository ChatMessage { get; }
		IClassroomRepository Classroom { get; }
		ILessonRepository Lesson { get; }
		IAttendanceRepository Attendance { get; }
		ISubscriptionRepository Subscription { get; }
		void Save();
	}
}
