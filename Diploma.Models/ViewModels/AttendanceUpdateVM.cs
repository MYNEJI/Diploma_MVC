using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Models.ViewModels
{
	public class AttendanceUpdateVM
	{
		public int LessonId { get; set; }
		public string StudentId { get; set; }
		public bool IsPresent { get; set; }
	}

}
