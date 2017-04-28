using Microsoft.AspNet.Identity.EntityFramework;

namespace LvivAdviser.Domain.Abstract
{
	public class Role : IdentityRole
	{
		public Role()
		{ }

		public Role(string name) : base(name) { }
	}
}