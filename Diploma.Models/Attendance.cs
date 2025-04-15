using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Models
{
	public class Attendance
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int LessonId { get; set; }
		[ForeignKey("LessonId")]
		[ValidateNever]
		public Lesson Lesson { get; set; }

		[Required]
		[ForeignKey("GroupStudentId")]
		[ValidateNever]
		public int GroupStudentId { get; set; }
		public GroupStudent GroupStudent { get; set; }

		[Required]
		[DisplayName("Is Present")]
		public bool IsPresent { get; set; }
	}
}
