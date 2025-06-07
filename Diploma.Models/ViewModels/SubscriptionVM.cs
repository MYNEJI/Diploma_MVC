using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Models.ViewModels
{
	public class SubscriptionVM
	{
		public Subscription Subscription { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> SubjectList { get; set; }
	}
}
