using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Models.ViewModels
{
	public class GroupStudentVM
	{
		public Group Group { get; set; }
		public IEnumerable<SelectListItem> StudentList { get; set; }
		public List<string> AssignedStudents { get; set; }
		public string SelectedStudentId { get; set; }
	}
}
