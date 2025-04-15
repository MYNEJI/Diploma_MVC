using Diploma.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Diploma.Models
{
	public class Lesson
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[ForeignKey("GroupId")]
		[ValidateNever]
		public int GroupId { get; set; }
		public Group Group { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		[DisplayName("Lesson Date")]
		public DateTime OriginalDate { get; set; } // Исходная дата урока

		[DataType(DataType.DateTime)]
		[DisplayName("New Lesson Date")]
		public DateTime? RescheduledDate { get; set; } // Новая дата урока (если перенесён)

		[Required]
		[DisplayName("Lesson Status")]
		public LessonStatus Status { get; set; }

		[ValidateNever]
		public List<Attendance> Attendances { get; set; }
	}
}
