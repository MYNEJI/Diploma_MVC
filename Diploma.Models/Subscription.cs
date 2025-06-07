using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Models
{
	public class Subscription
	{
		[Key]
		public int Id { get; set; }
		public string ApplicationUserId { get; set; }
		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
		[Required]
		public int SubjectId { get; set; }
		[ForeignKey("SubjectId")]
		[ValidateNever]
		public Subject Subject { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime? PaymentDate { get; set; }
		public string? SessionId { get; set; }
		public string? PaymentIntentId { get; set; }
	}
}