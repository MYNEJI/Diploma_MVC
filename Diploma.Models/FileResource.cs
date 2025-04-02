using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Diploma.Models
{
	public class FileResource
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public bool IsFolder { get; set; }
		public string FilePath { get; set; } // Path for file storage
		public int? ParentFolderId { get; set; }
		[ForeignKey("ParentFolderId")]
		public virtual FileResource ParentFolder { get; set; }
		[Required]
		public int GroupId { get; set; }
		[ForeignKey("GroupId")]
		[ValidateNever]
		public Group Group { get; set; }
	}
}