using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using Diploma.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiplomaWork.Areas.Teacher.Controllers
{
	[Area("Teacher")]
	[Authorize(Roles = "Manager, Admin, Teacher")]
	public class AttendanceController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public AttendanceController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index(int groupId, DateTime? month = null)
		{
			month ??= DateTime.Now;

			var lessons = _unitOfWork.Lesson
				.GetAll(l => l.GroupId == groupId && l.OriginalDate.Month == month.Value.Month && l.OriginalDate.Year == month.Value.Year)
				.ToList();

			var students = _unitOfWork.GroupStudent
				.GetAll(gs => gs.GroupId == groupId, includeProperties: "ApplicationUser")
			.Select(gs => gs.ApplicationUser)
				.ToList();

			var groupStudents = _unitOfWork.GroupStudent
				.GetAll(gs => gs.GroupId == groupId)
				.ToList();

			var attendances = _unitOfWork.Attendance
				.GetAll(a => lessons.Select(l => l.Id).Contains(a.LessonId))
				.ToList();

			var model = new AttendanceVM
			{
				Lessons = lessons,
				Students = students,
				GroupId = groupId,
				GroupStudents = groupStudents,
				Attendances = attendances,
				SelectedMonth = month.Value
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult SaveAllAttendances(int groupId, string month, List<AttendanceUpdateVM> attendanceData)
		{
			if (attendanceData == null || !attendanceData.Any())
			{
				return BadRequest("Нет данных для сохранения.");
			}

			foreach (var attendanceDto in attendanceData)
			{
				var groupStudent = _unitOfWork.GroupStudent
					.Get(gs => gs.ApplicationUserId == attendanceDto.StudentId && gs.GroupId == groupId);

				if (groupStudent == null)
				{
					return BadRequest($"Студент с ID {attendanceDto.StudentId} не найден в группе {groupId}.");
				}

				var attendance = _unitOfWork.Attendance
					.Get(a => a.LessonId == attendanceDto.LessonId && a.GroupStudentId == groupStudent.Id);

				if (attendance == null)
				{
					attendance = new Attendance
					{
						LessonId = attendanceDto.LessonId,
						GroupStudentId = groupStudent.Id,
						IsPresent = attendanceDto.IsPresent
					};
					_unitOfWork.Attendance.Add(attendance);
				}
				else
				{
					attendance.IsPresent = attendanceDto.IsPresent;
					_unitOfWork.Attendance.Update(attendance);
				}
			}

			_unitOfWork.Save();
			TempData["Success"] = "Посещаемость успешно сохранена!";
			return RedirectToAction(nameof(Index), new { groupId, month });
		}

	}
}