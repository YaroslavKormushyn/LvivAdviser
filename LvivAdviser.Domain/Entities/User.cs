using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace LvivAdviser.Domain.Entities
{
	enum Role
	{
		User,
		Moderator,
		Administrator
	}

	public class User : IdentityUser
	{
		public virtual IEnumerable<Rating> Ratings { get; set; }
	}
}
