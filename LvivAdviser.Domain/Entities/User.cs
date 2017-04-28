using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace LvivAdviser.Domain.Entities
{
	internal enum Role
	{
		User,
		UserModerator,
		ContentModerator,
		Administrator
	}

	public class User : IdentityUser
	{
		public virtual IEnumerable<Rating> Ratings { get; set; }
	}
}
