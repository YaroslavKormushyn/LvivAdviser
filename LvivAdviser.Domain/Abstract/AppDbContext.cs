using LvivAdviser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LvivAdviser.Domain.Abstract
{
	public class AppDbContext : DbContext
	{
		public AppDbContext() : base()
		{
		}

		public DbSet<Content> Contents { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
