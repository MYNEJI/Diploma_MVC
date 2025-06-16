using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using Diploma.Models.Enums;
using Diploma.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace DiplomaWork.Areas.Manager.Controllers
{
	[Area("Manager")]
	[Authorize]
	public class GroupController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly UserManager<IdentityUser> _userManager;

		public GroupController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
			_userManager = userManager;
		}
		public IActionResult Index(int subjectId)
		{
			var groups = _unitOfWork.Group
				.GetAll(g => g.SubjectId == subjectId, includeProperties: "Subject")
				.OrderByDescending(g => g.CreationDate)
				.ToList();

			ViewBag.SubjectId = subjectId;
			return View(groups);
		}

		/// <summary>
		/// Создание или обновление группы
		/// </summary>
		public IActionResult Upsert(int id, int subjectId)
		{
			ViewBag.SubjectId = subjectId;			
			Group groupObj = id == 0
				? new Group { CreationDate = DateTime.Now, SubjectId = subjectId }
				: _unitOfWork.Group.Get(u => u.Id == id, includeProperties: "Subject,SubjectTeacher,Classroom");

			var teachers = _unitOfWork.SubjectTeacher
				.GetAll(includeProperties: "ApplicationUser")
				.Select(teacher => new
				{
					Id = teacher.Id,
					Name = teacher.ApplicationUser.Name
				})
				.ToList();

			var classrooms = _unitOfWork.Classroom
				.GetAll()
				.Select(room => new
				{
					Id = room.Id,
					Name = room.RoomName
				})
				.ToList();

			ViewBag.SubjectTeacherList = new SelectList(teachers, "Id", "Name");
			ViewBag.ClassroomList = new SelectList(classrooms, "Id", "Name");
			ViewBag.WeekDays = Enum.GetValues(typeof(WeekDays)).Cast<WeekDays>().ToList();
			return View(groupObj);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(Group groupObj, List<WeekDays> WeekDays)
		{
			if (ModelState.IsValid)
			{
				groupObj.WeekDays = WeekDays;
				if (groupObj.Id == 0)
				{
					_unitOfWork.Group.Add(groupObj);
				}
				else
				{
					_unitOfWork.Group.Update(groupObj);
				}
				_unitOfWork.Save();
				return RedirectToAction("Index");
			}

			// Если валидация не прошла, нужно снова подготовить списки
			ViewBag.SubjectList = new SelectList(_unitOfWork.Subject.GetAll(), "Id", "Name");
			ViewBag.SubjectTeacherList = new SelectList(_unitOfWork.SubjectTeacher.GetAll(), "Id", "Name");
			ViewBag.ClassroomList = new SelectList(_unitOfWork.Classroom.GetAll(), "Id", "RoomName");
			ViewData["WeekDays"] = Enum.GetValues(typeof(WeekDays)).Cast<WeekDays>().ToList();
			return View(groupObj);
		}

		[HttpGet]
		public async Task<IActionResult> AssignStudents(int groupId)
		{
			var group = _unitOfWork.Group.Get(u => u.Id == groupId, includeProperties: "Subject");
			if (group == null)
			{
				return NotFound();
			}

			var studentIds = (await _userManager.GetUsersInRoleAsync("Student"))
					.Select(user => user.Id)
					.ToList();

			var studentUsers = _unitOfWork.ApplicationUser.GetAll()
								.Where(user => studentIds.Contains(user.Id))
								.ToList();

			GroupStudentVM vm = new GroupStudentVM
			{
				Group = group,
				StudentList = studentUsers.Select(user => new SelectListItem
				{
					Text = user.Name,
					Value = user.Id
				}),
				AssignedStudents = _unitOfWork.GroupStudent.GetAll(u => u.GroupId == groupId)
								  .Select(u => u.ApplicationUserId).ToList()
			};

			return View(vm);
		}

		[HttpPost]
		public IActionResult AssignStudents(GroupStudentVM vm)
		{
			vm.StudentList = _unitOfWork.ApplicationUser.GetAll().Select(user => new SelectListItem
			{
				Text = user.Name,
				Value = user.Id
			}).ToList();

			vm.AssignedStudents = _unitOfWork.GroupStudent.GetAll(u => u.GroupId == vm.Group.Id)
									 .Select(u => u.ApplicationUserId)
									 .ToList();

			// Логика назначения
			var existingAssignment = _unitOfWork.GroupStudent.GetAll(u =>
				u.GroupId == vm.Group.Id && u.ApplicationUserId == vm.SelectedStudentId).FirstOrDefault();

			if (existingAssignment == null)
			{
				_unitOfWork.GroupStudent.Add(new GroupStudent
				{
					GroupId = vm.Group.Id,
					ApplicationUserId = vm.SelectedStudentId
				});
				_unitOfWork.Save();
				TempData["success"] = "Student assigned successfully!";
			}
			else
			{
				TempData["error"] = "This student is already assigned to this group.";
			}

			return RedirectToAction(nameof(AssignStudents), new { groupId = vm.Group.Id });
		}

		[HttpPost]
		public IActionResult RemoveStudent(int groupId, string studentId)
		{
			var assignment = _unitOfWork.GroupStudent.GetAll(u =>
				u.GroupId == groupId && u.ApplicationUserId == studentId).FirstOrDefault();

			if (assignment != null)
			{
				_unitOfWork.GroupStudent.Remove(assignment);
				_unitOfWork.Save();
				TempData["success"] = "Student removed successfully!";
			}
			else
			{
				TempData["error"] = "This student is not assigned to this subject.";
			}

			return RedirectToAction(nameof(AssignStudents), new { groupId });
		}

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			List<Group> objGroupList = _unitOfWork.Group
				.GetAll(includeProperties: "Subject")
				.OrderBy(r => r.CreationDate)
				.ToList();

			return Json(new { data = objGroupList });
		}


		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var groupToBeDeleted = _unitOfWork.Group.Get(u => u.Id == id);
			if (groupToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			_unitOfWork.Group.Remove(groupToBeDeleted);
			_unitOfWork.Save();

			return Json(new { success = true, message = "Delete Successful" });
		}

		#endregion
	}
}
