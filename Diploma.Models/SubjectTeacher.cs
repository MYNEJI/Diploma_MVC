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
	public class SubjectTeacher
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
		[NotMapped]
		public string FullName => $"{ApplicationUser?.Name}";

	}
}
