using Diploma.DataAccess.Data;
using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using Diploma.Models.Enums;
using Diploma.Models.ViewModels;
using Diploma.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace DiplomaWork.Areas.Manager.Controllers
{
	[Area("Manager")]
	[Authorize(Roles = "Manager, Admin, Teacher")]
	public class LessonsController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public LessonsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult GenerateLessonsForMonth(int groupId)
		{
			var group = _unitOfWork.Group.Get(
				g => g.Id == groupId
			);

			if (group == null)
			{
				return NotFound();
			}

			// Получаем список дней недели из группы
			var weekDays = group.WeekDays;
			if (weekDays == null || !weekDays.Any())
			{
				return BadRequest("У группы не указаны дни недели.");
			}

			// Текущая дата и дата конца месяца
			var startDate = DateTime.Now.Date;
			var endDate = startDate.AddMonths(1);

			// Получаем уже существующие уроки для группы в указанном диапазоне дат
			var existingLessons =  _unitOfWork.Lesson.GetAll(l => l.GroupId == groupId && l.OriginalDate >= startDate && l.OriginalDate <= endDate)
				.Select(l => l.OriginalDate)
				.ToList();

			// Список уроков для добавления
			var lessonsToAdd = new List<Lesson>();

			for (var date = startDate; date <= endDate; date = date.AddDays(1))
			{
				if (weekDays.Contains((WeekDays)date.DayOfWeek) && !existingLessons.Contains(date)) // Проверка на день недели и отсутствие в БД
				{
					lessonsToAdd.Add(new Lesson
					{
						GroupId = group.Id,
						OriginalDate = date,
						Status = LessonStatus.Planned
					});
				}
			}

			// Если нет новых уроков для добавления
			if (!lessonsToAdd.Any())
			{
				return Ok("Уроки уже созданы на указанный период.");
			}

			// Сохраняем в базу данных
			_unitOfWork.Lesson.AddRange(lessonsToAdd);
			_unitOfWork.Save();

			return Ok($"Сгенерировано {lessonsToAdd.Count} новых уроков для группы {group.Name}.");
		}


		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			IEnumerable<SubjectTeacher> subjectList = _unitOfWork.SubjectTeacher.GetAll(u => u.ApplicationUserId == userId);
			List<int> subjectTeacherIds = subjectList.Select(st => st.Id).ToList();
			List<Group> objGroupList = _unitOfWork.Group.GetAll(u => subjectTeacherIds.
			Contains(u.SubjectTeacherId), includeProperties: "Subject"
				).ToList();
			return View(objGroupList);
		}

		public IActionResult Students(int groupId)
		{
			var students = _unitOfWork.GroupStudent
				.GetAll(u => u.GroupId == groupId, includeProperties: "ApplicationUser,Group")
				.ToList();

			if (!students.Any())
			{
				ViewBag.Message = "Студенты для указанной группы не найдены.";
				return View(new List<GroupStudent>());
			}

			return View(students);
		}
	}
}