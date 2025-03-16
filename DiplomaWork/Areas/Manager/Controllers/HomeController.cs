using System.Diagnostics;
using System.Security.Claims;
using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaWork.Areas.Manager.Controllers
{
	[Area("Manager")]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			IEnumerable<Subject> subjectList = _unitOfWork.Subject.GetAll(includeProperties: "Category");
			return View(subjectList);
		}

		public IActionResult Details(int subjectId)
		{
			CourseEnrollmentRequest request = new()
			{
				Subject = _unitOfWork.Subject.Get(u => u.Id == subjectId, includeProperties: "Category"),
				SubjectId = subjectId
			};
			return View(request);
		}

		[HttpPost]
		[Authorize]
		public IActionResult Details(CourseEnrollmentRequest enrollmentRequest)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			enrollmentRequest.ApplicationUserId = userId;
			enrollmentRequest.RequestDate = DateTime.Now;

			CourseEnrollmentRequest enrollmentRequestFromDb = _unitOfWork.CourseEnrollmentRequest
				.Get(u => u.ApplicationUserId == userId &&
			u.SubjectId == enrollmentRequest.SubjectId);

			_unitOfWork.CourseEnrollmentRequest.Add(enrollmentRequest);

			_unitOfWork.Save();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
