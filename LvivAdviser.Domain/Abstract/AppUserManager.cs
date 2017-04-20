using LvivAdviser.Domain.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LvivAdviser.Domain.Entities
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
			AppUserManager manager = new AppUserManager(new UserStore<User>(db));
			return manager;
		}
	}
}