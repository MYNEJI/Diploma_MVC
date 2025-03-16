using Diploma.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Models
{
	public class CourseEnrollmentRequest
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int SubjectId { get; set; } // Идентификатор курса
		[ForeignKey("SubjectId")]
		[ValidateNever]
		public Subject Subject { get; set; } // Связь с моделью курса
		public string ApplicationUserId { get; set; }
		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
		[Required]
		[DataType(DataType.DateTime)]
		public DateTime RequestDate { get; set; } // Дата подачи заявки
		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }
		[Required]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }
		[MaxLength(500)]
		[Display(Name = "Comment")]
		public string Comments { get; set; }
		[Required]
		public EnrollmentStatus Status { get; set; } = EnrollmentStatus.NotProcessed; // Статус заявки
	}
}
