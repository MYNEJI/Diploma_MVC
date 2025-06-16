using ClosedXML.Excel;
using Diploma.DataAccess.Repository;
using Diploma.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaWork.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class ReportController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ReportController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GenerateAttendanceReport(DateTime startDate, DateTime endDate)
		{
			// Получаем список всех групп
			var groups = _unitOfWork.Group.GetAll().ToList();

			// Создаем Excel-файл
			using (var workbook = new XLWorkbook())
			{
				foreach (var group in groups)
				{
					// Получаем уроки группы за указанный период
					var lessons = _unitOfWork.Lesson
						.GetAll(l => l.GroupId == group.Id && l.OriginalDate >= startDate && l.OriginalDate <= endDate)
						.ToList();

					var students = _unitOfWork.GroupStudent
						.GetAll(gs => gs.GroupId == group.Id, includeProperties: "ApplicationUser")
						.Select(gs => gs.ApplicationUser)
						.ToList();

					var attendances = _unitOfWork.Attendance
						.GetAll(a => lessons.Select(l => l.Id).Contains(a.LessonId))
						.ToList();

					// Создаем отдельный лист для группы
					var worksheet = workbook.Worksheets.Add($"Группа {group.Name}");

					// Заголовки
					worksheet.Cell(1, 1).Value = "Имя студента";
					worksheet.Cell(1, 2).Value = "Дата урока";
					worksheet.Cell(1, 3).Value = "Статус присутствия";

					int row = 2;
					foreach (var lesson in lessons)
					{
						foreach (var student in students)
						{
							var groupStudent = _unitOfWork.GroupStudent
									.Get(gs => gs.ApplicationUserId == student.Id);
							var attendance = attendances.FirstOrDefault(a => a.LessonId == lesson.Id && groupStudent.ApplicationUserId == student.Id);
							worksheet.Cell(row, 1).Value = student.Name; // Имя студента
							worksheet.Cell(row, 2).Value = lesson.OriginalDate.ToShortDateString(); // Дата урока
							worksheet.Cell(row, 3).Value = attendance != null && attendance.IsPresent ? "Присутствовал" : "Отсутствовал";
							row++;
						}
					}

					worksheet.Columns().AdjustToContents();
				}

				// Создаем Excel-файл и сохраняем его в поток
				using (var stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					stream.Position = 0; // Устанавливаем позицию потока в начало

					// Возвращаем файл для скачивания
					return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AttendanceReport_AllGroups.xlsx");
				}

				return RedirectToAction(nameof(Index));
			}
		}
	}
}
