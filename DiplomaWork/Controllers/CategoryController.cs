using Microsoft.AspNetCore.Mvc;

namespace DiplomaWork.Controllers
{
	public class CategoryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
