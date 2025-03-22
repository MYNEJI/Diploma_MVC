using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models.ViewModels;
using Diploma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaWork.Areas.Manager.Controllers
{
	[Area("Manager")]
	//[Authorize]
	public class RequestController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public RequestController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			List<CourseEnrollmentRequest> objRequestList = _unitOfWork.CourseEnrollmentRequest
				.GetAll(includeProperties: "Subject").OrderByDescending(r => r.RequestDate).ToList();

			return View(objRequestList);
		}

		/// <summary>
		/// Создание
		/// </summary>
		public IActionResult Upsert(int? id)
		{
			CourseEnrollmentRequest requestObj = _unitOfWork.CourseEnrollmentRequest.Get(
				u => u.Id == id,
				includeProperties: "Subject");

			return View(requestObj);
		}
		[HttpPost]
		public IActionResult Upsert(CourseEnrollmentRequest requestObj)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			// Fetch the existing request from the database based on the provided Id
			var existingRequest = _unitOfWork.CourseEnrollmentRequest
				.Get(u => u.Id == requestObj.Id);

			if (existingRequest != null)
			{
				// Update only the Status field
				existingRequest.Status = requestObj.Status;

				// Save the changes
				_unitOfWork.CourseEnrollmentRequest.Update(existingRequest);
				_unitOfWork.Save();

				TempData["success"] = "Status updated successfully!";
				return RedirectToAction("Index");
			}

			TempData["error"] = "Request not found.";
			return RedirectToAction("Index");
		}

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			List<CourseEnrollmentRequest> objRequestList = _unitOfWork.CourseEnrollmentRequest
				.GetAll(includeProperties: "Subject")
				.OrderBy(r => r.RequestDate)
				.ToList();
			return Json(new { data = objRequestList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var requestToBeDeleted = _unitOfWork.CourseEnrollmentRequest.Get(u => u.Id == id);
			if (requestToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			_unitOfWork.CourseEnrollmentRequest.Remove(requestToBeDeleted);
			_unitOfWork.Save();

			return Json(new { success = true, message = "Delete Successful" });
		}

		#endregion
	}
}
