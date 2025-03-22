using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Models
{
	public class GroupWeekday
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int WeekdayId { get; set; }
		[ForeignKey("WeekdayId")]
		[ValidateNever]
		public Weekday Weekday { get; set; }
		[Required]
		public int GroupId { get; set; }
		[ForeignKey("GroupId")]
		[ValidateNever]
		public Group Group { get; set; }
	}
}
