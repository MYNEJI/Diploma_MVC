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
			ViewBag.GroupId = groupId;
			var resources = _unitOfWork.FileResource.GetAll(u => u.GroupId == groupId);
			return View(resources);
		}

		[HttpPost]
		public IActionResult CreateFolder(int groupId, string folderName)
		{
			var folder = new FileResource
			{
				Name = folderName,
				IsFolder = true,
				GroupId = groupId
			};
			_unitOfWork.FileResource.Add(folder);
			_unitOfWork.Save();
			return RedirectToAction("Index", new { groupId });
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

	}
}
