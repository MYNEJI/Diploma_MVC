using Diploma.DataAccess.Data;
using Diploma.Models;
using Diploma.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Diploma.Utility;
using Diploma.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaWork.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class ClassroomController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ClassroomController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
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
		/// Создание
		/// </summary>
		public IActionResult Upsert(int? id)
		{
			Classroom classroom = new Classroom();
			if (id == null || id == 0)
			{
				return View(classroom);
			}
			else
			{
				Classroom classroomFromDb = _unitOfWork.Classroom.Get(u => u.Id == id);
				if (classroomFromDb == null)
				{
					return NotFound();
				}
				return View(classroomFromDb);
			}
		}
		[HttpPost]
		public IActionResult Upsert(Classroom obj)
		{
			if (ModelState.IsValid)
			{
				if (obj.Id == 0)
				{
					_unitOfWork.Classroom.Add(obj);
					TempData["success"] = "Classroom added successfully";
				}
				else
				{
					_unitOfWork.Classroom.Update(obj);
					TempData["success"] = "Classroom updated successfully";
				}

				_unitOfWork.Save();
				return RedirectToAction("Index");
			}
			return View();
		}

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			List<Classroom> objClassroomList = _unitOfWork.Classroom.GetAll().ToList();
			return Json(new { data = objClassroomList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var classroomToBeDeleted = _unitOfWork.Classroom.Get(u => u.Id == id);
			if (classroomToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}
			
			_unitOfWork.Classroom.Remove(classroomToBeDeleted);
			_unitOfWork.Save();

			return Json(new { success = true, message = "Delete Successful" });
		}

		#endregion
	}
}