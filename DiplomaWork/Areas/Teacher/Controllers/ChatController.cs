using Diploma.DataAccess.Data;
using Diploma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DiplomaWork.Areas.Teacher.Controllers
{
	[Area("Teacher")]
	[Authorize]
	public class ChatController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ChatController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index(int groupId)
		{
			var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);
			if (group == null)
			{
				return NotFound();
			}

			var messages = _context.ChatMessages
				.Where(m => m.GroupId == group.Id)
				.Include(m => m.Sender)
				.OrderBy(m => m.Timestamp)
				.ToList();


			ViewBag.Messages = messages;
			ViewBag.GroupName = group.Name;
			ViewBag.GroupId = group.Id;

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken] // Проверка CSRF-токена
		public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto chatMessageDto)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			var user = _context.ApplicationUsers.FirstOrDefault(g => g.Id == userId);
			if (user == null || string.IsNullOrWhiteSpace(chatMessageDto.Message))
			{
				return Json(new { success = false });
			}

			var group = _context.Groups.FirstOrDefault(g => g.Id == chatMessageDto.GroupId);
			if (group == null)
			{
				return Json(new { success = false });
			}

			var chatMessage = new ChatMessage
			{
				SenderId = user.Id,
				GroupId = chatMessageDto.GroupId,
				Message = chatMessageDto.Message,
				Timestamp = DateTime.Now
			};

			_context.ChatMessages.Add(chatMessage);
			await _context.SaveChangesAsync();

			return Json(new
			{
				success = true,
				message = chatMessage.Message,
				senderName = user.UserName,
				timestamp = chatMessage.Timestamp.ToShortTimeString()
			});
		}

	}
}
