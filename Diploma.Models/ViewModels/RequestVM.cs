using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Models.ViewModels
{
	public class CourseEnrollmentRequestVM
	{
		public CourseEnrollmentRequest CourseEnrollmentRequest { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> StatusList { get; set; }
	}
}
