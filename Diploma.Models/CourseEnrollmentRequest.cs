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
	internal class CourseEnrollmentRequest
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int SubjectId { get; set; } // Идентификатор курса

		[ForeignKey("SubjectId")]
		[ValidateNever]
		public Subject Subject { get; set; } // Связь с моделью курса
		[Required]
		[DataType(DataType.DateTime)]
		public DateTime RequestDate { get; set; } // Дата подачи заявки
		[MaxLength(500)]
		public string Comments { get; set; }
	}
}
