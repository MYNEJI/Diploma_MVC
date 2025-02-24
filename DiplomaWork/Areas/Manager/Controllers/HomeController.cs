using System.Diagnostics;
using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
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
			Subject subject = _unitOfWork.Subject.Get(u => u.Id == subjectId, includeProperties: "Category");
			return View(subject);
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
