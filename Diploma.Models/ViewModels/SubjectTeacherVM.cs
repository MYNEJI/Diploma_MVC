using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Models.ViewModels
{
	public class SubjectTeacherVM
	{
		public Subject Subject { get; set; }
		public IEnumerable<SelectListItem> TeacherList { get; set; }
		public List<string> AssignedTeachers { get; set; }
		public string SelectedTeacherId { get; set; }
	}
}
