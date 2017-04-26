namespace LvivAdviser.Domain.Migrations
{
	using LvivAdviser.Domain.Abstract;
	using LvivAdviser.Domain.Entities;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<LvivAdviser.Domain.Abstract.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LvivAdviser.Domain.Abstract.AppDbContext context)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<User>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<IdentityRole>(context));

	        var contentList = new List<Content>
	        {
		        new Content
		        {
			        Id = 1,
			        Name = "Black cat",
			        Type = LvivAdviser.Domain.Entities.Type.Food,
			        Description = "restaurant",
			        MainPhoto = null,
			        Rating = 5
		        },
		        new Content
		        {
			        Id = 2,
			        Name = "Celentano",
			        Type = LvivAdviser.Domain.Entities.Type.Food,
			        Description = "restaurant",
			        MainPhoto = null,
			        Rating = 4
		        },
		        new Content
		        {
			        Id = 3,
			        Name = "Da Vinci",
			        Type = LvivAdviser.Domain.Entities.Type.Food,
			        Description = "restaurant/pizza",
			        MainPhoto = null,
			        Rating = 4
		        },
		        new Content
		        {
			        Id = 4,
			        Name = "Kredense",
			        Type = LvivAdviser.Domain.Entities.Type.Food,
			        Description = "cafe",
			        MainPhoto = null,
			        Rating = 3
		        }
	        };
	        context.Contents.AddRange(contentList);

			string[] roleNames = 
				Enum.GetNames(typeof(Entities.Role))
				.Select(name => name + 's').ToArray();
	        string adminRole = "Administrators";
	        string adminName = "admin";
	        string password = "MySecret";
	        string email = "admin@example.com";

			if (!roleNames.Contains(adminRole))
	        {
		        roleNames.ToList().Add(adminRole);
	        }
	        
	        foreach (var role in roleNames)
	        {
				if (!roleMgr.RoleExists(role))
				{
					roleMgr.Create(new IdentityRole(role));
				}
			}

	        User user = userMgr.FindByName(adminName);
	        if (user == null)
	        {
		        var ident = userMgr.Create(new User { UserName = adminName, Email = email },
			        password);
		        if (!ident.Succeeded)
		        {
			        throw new Exception(ident.Errors.Aggregate((n, err) =>
			        {
				        return n + Environment.NewLine + err;
			        }));
		        }
		        user = userMgr.FindByName(adminName);
	        }

	        if (!userMgr.IsInRole(
				user.Id, 
				adminRole))
	        {
		        userMgr.AddToRole(user.Id, adminRole);
	        }

	        context.SaveChanges();
        }
    }
}
