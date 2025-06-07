using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Diploma.Models
{
	public class Subject
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		public string Description { get; set; }
		[Required]
		public string ISBN { get; set; }
		[Required]
		[Display(Name = "Price")]
		[Range(1, 1000)]
		public double Price { get; set; }
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }
		[ValidateNever]
		public string ImageUrl { get; set; }
	}
}
