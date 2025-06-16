using Diploma.DataAccess.Data;
using Diploma.Models;
using Diploma.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Diploma.Utility;
using System.Text.RegularExpressions;
using Group = Diploma.Models.Group;
using Diploma.Models.Enums;

namespace DiplomaWork.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ScheduleController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ScheduleController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			List<Classroom> objClassroomList = _unitOfWork.Classroom.GetAll().ToList();
			return View(objClassroomList);
		}

		/// <summary>
		/// Вывод расписания для конкретной аудитории
		/// </summary>
		public IActionResult ScheduleList(int id)
		{
			//Classroom classroomFromDb = _unitOfWork.Classroom.Get(u => u.Id == id);
			//if (classroomFromDb == null)
			//{
			//	return NotFound();
			//}
			//return View(classroomFromDb);


			//List<Group> schedule = _unitOfWork.Group.GetAll(g => g.ClassroomId == id, includeProperties: "Subject,SubjectTeacher,Classroom").ToList();

			//if (!schedule.Any())
			//{
			//	return NotFound($"No schedule found for Classroom ID: {id}");
			//}

			//return View(schedule);

			// Получаем расписание для аудитории
			List<Group> schedule = _unitOfWork.Group.GetAll( g => g.ClassroomId == id,
									includeProperties: "Subject,SubjectTeacher").ToList();

			// Создаем словарь, группируя данные по дням недели
			var weekSchedule = new Dictionary<WeekDays, List<Group>>();

			foreach (WeekDays day in Enum.GetValues(typeof(WeekDays)))
			{
				weekSchedule[day] = schedule.Where(g => g.WeekDays.Contains(day)).ToList();
			}

			ViewBag.Name = _unitOfWork.Classroom.Get(u => u.Id == id)?.RoomName; // Возвращает имя аудитории
			return View(weekSchedule);
		}
	}
}