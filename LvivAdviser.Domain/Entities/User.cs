using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
		[Range(0.0, double.PositiveInfinity)]
		[DefaultValue(0.0)]
		public decimal Budget { get; set; }

		public virtual IEnumerable<Content> Favourites { get; set; }
		public virtual IEnumerable<Rating> Ratings { get; set; }
	}
}
