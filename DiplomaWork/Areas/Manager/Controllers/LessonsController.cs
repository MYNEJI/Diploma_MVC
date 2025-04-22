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
				if (weekDays.Contains((WeekDays)date.DayOfWeek) 
					//&& !existingLessons.Contains(date)
					) // Проверка на день недели и отсутствие в БД
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

			// Создаем записи посещаемости для новых уроков
			var studentsInGroup = _unitOfWork.GroupStudent.GetAll(gs => gs.GroupId == groupId).ToList();
			if (!studentsInGroup.Any())
			{
				return BadRequest("У группы нет студентов для создания записей посещаемости.");
			}

			var attendancesToAdd = new List<Attendance>();
			foreach (var lesson in lessonsToAdd)
			{
				foreach (var student in studentsInGroup)
				{
					attendancesToAdd.Add(new Attendance
					{
						LessonId = lesson.Id,
						GroupStudentId = student.Id,
						IsPresent = false // Изначально отмечаем всех как отсутствующих
					});
				}
			}

			// Сохраняем записи посещаемости
			_unitOfWork.Attendance.AddRange(attendancesToAdd);
			_unitOfWork.Save();

			return Ok($"Сгенерировано {lessonsToAdd.Count} новых уроков для группы {group.Name}.");
		}
	}
}