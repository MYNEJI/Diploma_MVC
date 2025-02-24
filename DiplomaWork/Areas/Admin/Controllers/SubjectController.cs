using Diploma.DataAccess.Data;
using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using Diploma.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DiplomaWork.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SubjectController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public SubjectController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			List<Subject> objSubjectList = _unitOfWork.Subject.GetAll(includeProperties: "Category").ToList();
			return View(objSubjectList);
		}

		/// <summary>
		/// Создание
		/// </summary>
		public IActionResult Upsert(int? id)
		{
			SubjectVM subjectVM = new()
			{
				CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
				Subject = new Subject()
			};
			if (id == null || id == 0)
			{
				// Create
				return View(subjectVM);
			}
			else
			{
				// Update
				subjectVM.Subject = _unitOfWork.Subject.Get(u => u.Id == id);
				return View(subjectVM);
			}
		}
		[HttpPost]
		public IActionResult Upsert(SubjectVM subjectVM, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string subjectPath = Path.Combine(wwwRootPath, @"images\subject");

					if (!string.IsNullOrEmpty(subjectVM.Subject.ImageUrl))
					{
						// Delete the old image
						var oldImagePath =
							Path.Combine(wwwRootPath, subjectVM.Subject.ImageUrl.TrimStart('\\'));

						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}


					using (var fileStream = new FileStream(Path.Combine(subjectPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					subjectVM.Subject.ImageUrl = @"\images\subject\" + fileName;
				}

				if (subjectVM.Subject.Id == 0)
				{
					_unitOfWork.Subject.Add(subjectVM.Subject);
				}
				else
				{
					_unitOfWork.Subject.Update(subjectVM.Subject);
				}

				_unitOfWork.Save();
				TempData["success"] = "Subject created successfully";
				return RedirectToAction("Index");
			}
			else
			{
				subjectVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});
				return View(subjectVM);
			}
		}

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			List<Subject> objSubjectList = _unitOfWork.Subject.GetAll(includeProperties: "Category").ToList();
			return Json(new { data = objSubjectList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var subjectToBeDeleted = _unitOfWork.Subject.Get(u => u.Id == id);
			if (subjectToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			// Delete the old image
			var oldImagePath =
				Path.Combine(_webHostEnvironment.WebRootPath,
				subjectToBeDeleted.ImageUrl.TrimStart('\\'));

			if (System.IO.File.Exists(oldImagePath))
			{
				System.IO.File.Delete(oldImagePath);
			}

			_unitOfWork.Subject.Remove(subjectToBeDeleted);
			_unitOfWork.Save();

			return Json(new { success = true, message = "Delete Successful" });
		}

		#endregion
	}
}