using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LvivAdviser.Domain.Entities
{
	public enum Role
	{
		User = 1,
		Moderator,
		Admin
	}
	
	[Table("User")]
	public class User
	{
		[Key]
		public int ID { get; set; }

		public string Email { get; set; }

		public string Login { get; set; }

		public string Password { get; set; }

		public Role Role { get; set; }

		public virtual IEnumerable<Rating> Ratings { get; set; }
	}
}
