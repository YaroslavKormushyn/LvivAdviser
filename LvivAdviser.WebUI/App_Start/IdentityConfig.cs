﻿using LvivAdviser.Domain.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace LvivAdviser.WebUI
{
	public class IdentityConfig
	{
		public void Configuration(IAppBuilder app)
		{
			app.CreatePerOwinContext(AppDbContext.Create);
			app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
			app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);

			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login"),
			});
		}
	}
}