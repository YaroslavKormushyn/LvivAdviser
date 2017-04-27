using LvivAdviser.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace LvivAdviser.Domain.Abstract
{
	public class AppDbContext : IdentityDbContext<User>
	{
		public AppDbContext() : base("name=LvivAdviserConnection")
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

        public System.Data.Entity.DbSet<LvivAdviser.Domain.Abstract.Role> IdentityRoles { get; set; }
    }

    public class IdentityDbInit
		: NullDatabaseInitializer<AppDbContext>
	{
	}
}

