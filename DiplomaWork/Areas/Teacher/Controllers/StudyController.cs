using Diploma.DataAccess.Data;
using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using Diploma.Models.ViewModels;
using Diploma.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;

namespace DiplomaWork.Areas.Teacher.Controllers
{
	[Area("Teacher")]
	[Authorize(Roles = "Manager, Admin, Teacher")]
	public class StudyController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public StudyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
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
			// Получаем студентов для указанной группы
			var students = _unitOfWork.GroupStudent
				.GetAll(u => u.GroupId == groupId, includeProperties: "ApplicationUser,Group")
				.ToList(); // Преобразуем к List<GroupStudent>

			// Проверяем наличие студентов
			if (!students.Any())
			{
				ViewBag.Message = "Студенты для указанной группы не найдены.";
				return View(new List<GroupStudent>());
			}

			return View(students); // Передаем List<GroupStudent> в представление
		}
	}
}