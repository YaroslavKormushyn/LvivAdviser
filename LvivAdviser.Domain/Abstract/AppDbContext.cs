using LvivAdviser.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace LvivAdviser.Domain.Abstract
{
	public class AppDbContext : IdentityDbContext<User>
	{
		public AppDbContext() : base("LvivAdviser")
		{
		}

		public DbSet<Content> Contents { get; set; }
		public DbSet<Rating> Ratings { get; set; }

		static AppDbContext()
		{
			Database.SetInitializer<AppDbContext>(new IdentityDbInit());
		}

		public static AppDbContext Create()
		{
			return new AppDbContext();
		}
	}

	public class IdentityDbInit
		: DropCreateDatabaseIfModelChanges<AppDbContext>
	{
		protected override void Seed(AppDbContext context)
		{
			PerformInitialSetup(context);
			base.Seed(context);
		}
		public void PerformInitialSetup(AppDbContext context)
		{
			// initial configuration will go here
		}
	}
}

