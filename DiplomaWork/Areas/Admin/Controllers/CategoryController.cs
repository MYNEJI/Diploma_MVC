﻿using Diploma.DataAccess.Data;
using Diploma.Models;
using Diploma.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Diploma.Utility;

namespace DiplomaWork.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class CategoryController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public CategoryController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
			return View(objCategoryList);
		}
		/// <summary>
		/// Создание
		/// </summary>
		/// <returns></returns>
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Category obj)
		{
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
			}
			if (ModelState.IsValid)
			{
				_unitOfWork.Category.Add(obj);
				_unitOfWork.Save();
				TempData["success"] = "Category created successfully";
				return RedirectToAction("Index");
			}
			return View();
		}

		/// <summary>
		/// Обновление
		/// </summary>
		/// <returns></returns>
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Category categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
			if (categoryFromDb == null)
			{
				return NotFound();
			}
			return View(categoryFromDb);
		}
		[HttpPost]
		public IActionResult Edit(Category obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Category.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Category updated successfully";
				return RedirectToAction("Index");
			}
			return View();
		}

		/// <summary>
		/// Удаление
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Category categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
			if (categoryFromDb == null)
			{
				return NotFound();
			}
			return View(categoryFromDb);
		}
		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
			if (obj == null)
			{
				return NotFound();
			}
			_unitOfWork.Category.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index");
		}
	}
}