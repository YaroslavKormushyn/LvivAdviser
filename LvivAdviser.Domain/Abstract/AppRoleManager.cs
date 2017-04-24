using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace LvivAdviser.Domain.Abstract
{
	public class AppRoleManager : RoleManager<IdentityRole>, IDisposable
	{
		public AppRoleManager(RoleStore<IdentityRole> store)
			: base(store)
		{
		}

		public static AppRoleManager Create(
			IdentityFactoryOptions<AppRoleManager> options,
			IOwinContext context)
		{
			return new AppRoleManager(new
				RoleStore<IdentityRole>(context.Get<AppDbContext>()));
		}
	}
}