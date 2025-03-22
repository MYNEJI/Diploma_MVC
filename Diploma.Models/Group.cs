using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using Diploma.Models.ViewModels;
using Diploma.Models.Enums;

namespace Diploma.Models
{
	public class Group
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(30)]
		[DisplayName("Group Name")]
		public string Name { get; set; }

		[Required]
		public int SubjectId { get; set; }
		[ForeignKey("SubjectId")]
		[ValidateNever]
		public Subject Subject { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime CreationDate { get; set; }
		[Required]
		public string StartTime { get; set; }
		[Required]
		public string EndTime { get; set; }
		[Required]
		public int SubjectTeacherId { get; set; }
		[ForeignKey("SubjectTeacherId")]
		[ValidateNever]
		public SubjectTeacher SubjectTeacher { get; set; }
		[Required]
		public List<WeekDays> WeekDays { get; set; }
	}
}
