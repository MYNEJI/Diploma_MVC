using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaWork.Areas.Teacher.Controllers
{
	[Area("Teacher")]
	[Authorize]
	public class FileManagerController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public FileManagerController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index(int groupId)
		{
			var group = _unitOfWork.Group.Get(u => u.Id == groupId);
			ViewBag.GroupName = group.Name;
			ViewBag.GroupId = groupId;
			var resources = _unitOfWork.FileResource.GetAll(u => u.GroupId == groupId);			
			return View(resources);
		}

		[HttpPost]
		public IActionResult UploadFile(int groupId, IFormFile file, int? parentFolderId)
		{
			if (file != null)
			{
				string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
				Directory.CreateDirectory(uploadPath);

				string filePath = Path.Combine(uploadPath, file.FileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyTo(stream);
				}

				// Проверка существования файла
				if (System.IO.File.Exists(filePath))
				{
					TempData["Error"] = $"A file with the name '{file.FileName}' already exists.";
					return RedirectToAction("Index", new { groupId });
				}

				var fileResource = new FileResource
				{
					Name = file.FileName,
					IsFolder = false,
					FilePath = filePath,
					GroupId = groupId,
					ParentFolderId = parentFolderId
				};
				_unitOfWork.FileResource.Add(fileResource);
				_unitOfWork.Save();
			}
			return RedirectToAction("Index", new { groupId });
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			var resource = _unitOfWork.FileResource.Get(u => u.Id == id);
			if (resource == null)
			{
				return NotFound();
			}

			_unitOfWork.FileResource.Remove(resource);
			_unitOfWork.Save();
			return RedirectToAction("Index", new { groupId = resource.GroupId });
		}
		public IActionResult UploadFile(int groupId, IFormFile file)
		{
			if (file != null && file.Length > 0)
			{
				var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", file.FileName);

				// Save the file to the server
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyTo(stream);
				}

				// Create and save a new FileResource record in the database
				var fileResource = new FileResource
				{
					GroupId = groupId,
					Name = file.FileName,
					FilePath = file.FileName, // Store the file name or relative path in the database
					IsFolder = false // Set whether it's a file or folder
				};
				_unitOfWork.FileResource.Add(fileResource);
				_unitOfWork.Save();
			}

			return RedirectToAction("Index", new { groupId = groupId });
		}

		public IActionResult Download(int id)
		{
			// Найдите ресурс по ID
			var resource = _unitOfWork.FileResource.Get(u => u.Id == id);

			if (resource == null || resource.IsFolder)
			{
				return NotFound("File not found or it's a folder.");
			}

			// Полный путь к файлу
			var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", resource.FilePath);

			if (!System.IO.File.Exists(filePath))
			{
				return NotFound("File does not exist on the server.");
			}

			// Возвращаем файл
			var contentType = "application/octet-stream"; // По умолчанию бинарный файл
			var fileName = resource.Name;

			return PhysicalFile(filePath, contentType, fileName);
		}
	}
}