using static LvivAdviser.Domain.Entities.Type;

namespace LvivAdviser.Domain.Migrations
{
	using Abstract;
	using Entities;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AppDbContext context)
        {
	        AppUserManager userMgr = new AppUserManager(new UserStore<User>(context));
	        AppRoleManager roleMgr = new AppRoleManager(new RoleStore<Abstract.Role>(context));

	        //Seed Content
	        var contentList = new List<Content>
	        {
		         new Content
		        {
			        Name = "Black Cat",
			        Type = Food,
			        Description = "Restaurant",
			        MainPhoto = null
		        },
		        new Content
		        {
			        Name = "Celentano",
			        Type = Food,
			        Description = "Restaurant",
			        MainPhoto = null
		        },
		        new Content
		        {
			        Name = "Da Vinci",
			        Type = Food,
			        Description = "Restaurant/Pizza",
			        MainPhoto = null
		        },
		        new Content
		        {
			        Name = "Kredens",
			        Type = Food,
			        Description = "Cafe",
			        MainPhoto = null
		        },
		        new Content
		        {
			        Type = Rest,
			        Name = "Hotel \"Lviv\"",
			        Description = "Hotel",
			        MainPhoto = null
		        },
		        new Content
		        {
			        Type = Rest,
			        Name = "Myroslav's Hostel",
			        Description = "Stay/B&B",
			        MainPhoto = null
		        },
                new Content
                {
                    Name = "IMAX",
                    Type = FreeTime,
                    Description = "Cinema",
                    MainPhoto = null
                },
                new Content
                {
                    Name = "Multiplex",
                    Type = FreeTime,
                    Description = "Cinema",
                    MainPhoto = null
                },
                new Content
                {
                    Name = "Kinopalats",
                    Type = FreeTime,
                    Description = "Cinema",
                    MainPhoto = null
                },
                new Content
                {
                    Name = "Medyk",
                    Type = FreeTime,
                    Description = "Icerink",
                    MainPhoto = null
                },
                new Content
                {
                    Name = "Dnister",
                    Type = Rest,
                    Description = "Hotel",
                    MainPhoto = null
                },
                new Content
                {
                    Name = "Atlas",
                    Type = Rest,
                    Description = "Hotel",
                    MainPhoto = null
                },
                new Content
                {
                    Name = "Astoria",
                    Type = Rest,
                    Description = "Hotel",
                    MainPhoto = null
                }
	        };

	        foreach (var content in contentList)
	        {
		        if (!context.Contents.Any(cont => 
					cont.Name == content.Name && cont.Type == content.Type))
		        {
			        context.Contents.Add(content);
		        }
	        }

	        string[] roleNames =
		        Enum.GetNames(typeof(Entities.Role))
			        .Select(name => name + 's').ToArray();

	        string adminRole = "Administrators";

	        if (!roleNames.Contains(adminRole))
	        {
		        roleNames.ToList().Add(adminRole);
	        }

	        //Seed Roles
	        foreach (var role in roleNames)
	        {
		        if (!roleMgr.RoleExists(role))
		        {
			        roleMgr.Create(new Abstract.Role(role));
		        }
	        }

	        string adminName = "admin";
	        string password = "password";
	        string email = "admin@example.com";

	        User user = userMgr.FindByName(adminName);
	        if (user == null)
	        {
		        var ident = userMgr.Create(new User { UserName = adminName, Email = email },
			        password);
		        if (!ident.Succeeded)
		        {
			        throw new Exception(ident.Errors.Aggregate(
						(n, err) => n + Environment.NewLine + err));
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
