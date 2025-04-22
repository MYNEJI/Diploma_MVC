using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Models.ViewModels
{
	public class AttendanceVM
	{
		public List<Lesson> Lessons { get; set; }
		public List<ApplicationUser> Students { get; set; }
		public List<GroupStudent> GroupStudents { get; set; }
		public int GroupId { get; set; }
		public List<Attendance> Attendances { get; set; }
		public DateTime SelectedMonth { get; set; }
	}
}
