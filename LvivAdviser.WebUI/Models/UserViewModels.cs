using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Entities;

namespace LvivAdviser.WebUI.Models
{
	public class CreateModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}

	public class EditModel
	{
		[Required]
		public string Id { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string Email { get; set; }

		[Required]
		public decimal Budget { get; set; }

		public string Password { get; set; }
	}

	public class LoginModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Password { get; set; }
	}

	public class SignUpModel
	{
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string Password { get; set; }

		[Compare("Password", ErrorMessage = "Passwords do not match. Try again.")]
		public string ConfirmPassword { get; set; }
	}

    public class RoleEditModel
    {
        public Role Role { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<User> NonMembers { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}