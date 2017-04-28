using LvivAdviser.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LvivAdviser.Domain.Abstract
{
	public class AppUserManager : UserManager<User>
	{
		public AppUserManager(IUserStore<User> store) : base(store)
		{
		}

		public static AppUserManager Create(
			IdentityFactoryOptions<AppUserManager> options,
			IOwinContext context)
		{
			AppDbContext db = context.Get<AppDbContext>();
			AppUserManager manager = new AppUserManager(new UserStore<User>(db))
			{
				PasswordValidator = new CustomPasswordValidator
				{
					RequiredLength = 6,
					RequireNonLetterOrDigit = false,
					RequireDigit = false,
					RequireLowercase = true,
					RequireUppercase = true
				}
			};

			manager.UserValidator = new CustomUserValidator(manager)
			{
				AllowOnlyAlphanumericUserNames = true,
				RequireUniqueEmail = true
			};

			return manager;
		}
	}
}