using Microsoft.AspNet.Identity.EntityFramework;

/// <summary>
/// This is a backup class if the <class>IdentityRole</class>
/// won't have it. 
/// </summary>
namespace LvivAdviser.Domain.Abstract
{
	public class Role : IdentityRole
	{
		public Role() : base() { }

		public Role(string name) : base(name) { }
	}
}