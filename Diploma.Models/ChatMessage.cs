using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Diploma.Models
{
	public class ChatMessage
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string SenderId { get; set; }

		[ForeignKey("SenderId")]
		[ValidateNever]
		public ApplicationUser Sender { get; set; }

		[Required]
		public int GroupId { get; set; }
		[ForeignKey("GroupId")]
		[ValidateNever]
		public Group Group { get; set; }

		[Required]
		public string Message { get; set; }

		[Required]
		public DateTime Timestamp { get; set; } = DateTime.Now;
	}

}
