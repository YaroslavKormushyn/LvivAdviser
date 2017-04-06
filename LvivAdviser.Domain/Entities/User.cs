using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LvivAdviser.Domain.Entities
{
	enum Role
	{
		User = 1,
		Moderator,
		Admin
	}
	
	[Table("User")]
	class User
	{
		[Key]
		public int ID { get; set; }

		public string Email { get; set; }

		[Index(IsUnique = true)]
		public string Login { get; set; }

		public string Password { get; set; }
		public Role Role { get; set; }

		public virtual IEnumerable<Rating> Ratings { get; set; }
	}
}
