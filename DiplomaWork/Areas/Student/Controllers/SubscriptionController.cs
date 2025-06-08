using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using Diploma.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace DiplomaWork.Areas.Student.Controllers
{
	[Area("Student")]
	[Authorize]
	public class SubscriptionController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public SubscriptionController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			var subscriptions = _unitOfWork.Subscription
				.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Subject")
				.Select(sub => new SubscriptionVM
				{
					Id = sub.Subject.Id,
					Title = sub.Subject.Title, // Предполагается, что у Subscription есть связь с Subject
					EndDate = sub.EndDate
				}).ToList();

			return View(subscriptions);
		}

		public IActionResult OrderConfirmation(int id)
		{
			Diploma.Models.Subscription subscription = _unitOfWork.Subscription
				.Get(u => u.Id == id, includeProperties: "ApplicationUser");
			var service = new SessionService();
			Session session = service.Get(subscription.SessionId);
			if (session.PaymentStatus.ToLower() == "paid")
			{
				var date = subscription.EndDate;
				subscription.EndDate = date.AddMonths(1);
				_unitOfWork.Subscription.Update(subscription);
				_unitOfWork.Save();
				TempData["success"] = "Payment successful";
				return RedirectToAction("Index", "Subscription");
			}
			else
			{
				TempData["error"] = "Payment not successful";
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		public IActionResult Upsert([FromForm] int subjectId)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;

				var subscription = _unitOfWork.Subscription.Get(u => u.ApplicationUserId == userId);
				if (subscription == null)
				{
					var createNewSubscription = new Diploma.Models.Subscription
					{
						ApplicationUserId = userId,
						SubjectId = subjectId,
						StartDate = DateTime.Now,
						EndDate = DateTime.Now
					};
					_unitOfWork.Subscription.Add(createNewSubscription);
					_unitOfWork.Save();
					subscription = _unitOfWork.Subscription.Get(u => u.ApplicationUserId == userId);
				}

				// Интеграция Stripe
				try
				{
					var domain = "http://localhost:5155";
					var options = new SessionCreateOptions
					{
						SuccessUrl = domain + $"/Student/Subscription/OrderConfirmation/{subscription.Id}",
						CancelUrl = domain + "/Student/Subscription",
						LineItems = new List<SessionLineItemOptions>(),
						Mode = "payment",
					};

					var item = _unitOfWork.Subject.Get(u => u.Id == subscription.SubjectId);
					var sessionLineItem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)item.Price * 100, // Предполагается, что у Subject есть свойство Price
							Currency = "usd",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Title// Предполагается, что у Subject есть свойство ImageUrl
							},
						},
						Quantity = 1,
					};
					options.LineItems.Add(sessionLineItem);

					var service = new SessionService();
					Session session = service.Create(options);
					_unitOfWork.Subscription.UpdateStripePaymentId(subscription.Id, session.Id, session.PaymentIntentId);
					_unitOfWork.Save();
					Response.Headers.Add("Location", session.Url);
					return new StatusCodeResult(303);
				}
				catch (Exception ex)
				{
					TempData["error"] = $"Error occurred while creating a payment session: {ex.Message}";
					return RedirectToAction("Index");
				}

				return RedirectToAction(nameof(OrderConfirmation), new { id = subscription.Id });
			}
			else
			{
				TempData["error"] = "Error occured while prolonging subscription";
				return RedirectToAction("Index");
			}
		}

		#region API CALLS

		[HttpGet]
		public IActionResult GetAll()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			// Get the subjects the user is subscribed to
			var groups = _unitOfWork.GroupStudent.GetAll(u => u.ApplicationUserId == userId);
			var groupIds = groups.Select(g => g.GroupId).ToList();
			var subjects = _unitOfWork.Group.GetAll(u => groupIds.Contains(u.Id)).ToList();
			var subscribedSubjects = _unitOfWork.Subject
				.GetAll(u => u.Id == subjects.Select(s => s.SubjectId).FirstOrDefault()).ToList();
			List<Subject> objSubjectList = subscribedSubjects;
			return Json(new { data = objSubjectList });
		}

		#endregion
	}
}
