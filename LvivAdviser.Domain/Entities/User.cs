using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LvivAdviser.Domain.Entities
{	
	[Table("User")]
	public class User : IdentityUser
	{
		public virtual IEnumerable<Rating> Ratings { get; set; }
	}
}
