using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Users
{
	public class IdentityConfig
	{
		public void Configuration(IAppBuilder app)
		{
			app.CreatePerOwinContext<AppDbContext>(AppDbContext.Create);
			app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login"),
			});
		}
	}
}