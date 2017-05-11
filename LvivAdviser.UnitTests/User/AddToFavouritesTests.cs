using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LvivAdviser.UnitTests.User
{
	[TestClass]
	public class AddToFavouritesTests
	{
		private Mock<IPrincipal> mockPrincipal;
		private string username = "test@example.com";

		[TestInitialize]
		public void Init()
		{
			this.mockPrincipal = new Mock<IPrincipal>();
			var identity = new GenericIdentity(this.username);
			var nameIdentifierClaim = new Claim(
				ClaimTypes.NameIdentifier, this.username);
			identity.AddClaim(nameIdentifierClaim);
			this.mockPrincipal.Setup(x => x.Identity).Returns(identity);
		}

		[TestMethod]
		public void AddNonExistentToFavourites()
		{
			var contentToFavourite = new Content
			{
				Id = 1
			};

			var favourited = new List<Content>
			{
				new Content
				{
					Id = 2
				}
			};

			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username,
				Favourites = favourited
			};

			var context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			var mockContentRepository
				= new Mock<IRepository<Content>>();
			mockContentRepository
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(It.Is<Content>(null));

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);
			userManager.Setup(m => m.UpdateAsync(user))
				.ReturnsAsync(IdentityResult.Success);

			var userController
				= new UserController(
					mockContentRepository.Object,
					null)
				{
					ControllerContext = context.Object,
					UserManager = userManager.Object
				};
			var actionResult = userController.AddToFavourites(contentToFavourite.Id);

			Assert.AreEqual(
				"No such content.",
				((string[])((ViewResult)actionResult).Model).Single());

			mockContentRepository.Verify(r => r.GetById(contentToFavourite.Id), Times.Once);
			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Never);
			userManager.Verify(r => r.UpdateAsync(user), Times.Never);
		}

		[TestMethod]
		public void AddNotFavouritedToFavourites()
		{
			var contentToFavourite = new Content
			{
				Id = 1
			};

			var favourited = new List<Content>
			{
				new Content
				{
					Id = 2
				}
			};

			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username,
				Favourites = favourited
			};

			var context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			var mockContentRepository
				= new Mock<IRepository<Content>>();
			mockContentRepository
				.Setup(x => x.GetById(contentToFavourite.Id))
				.Returns(contentToFavourite);

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);
			userManager.Setup(m => m.UpdateAsync(user))
				.ReturnsAsync(IdentityResult.Success);

			var userController
				= new UserController(
					mockContentRepository.Object,
					null)
				{
					ControllerContext = context.Object,
					UserManager = userManager.Object
				};
			Assert.AreEqual(1, favourited.Count);

			var actionResult = userController.AddToFavourites(contentToFavourite.Id);

			Assert.AreEqual(2, favourited.Count);

			mockContentRepository.Verify(r => r.GetById(contentToFavourite.Id), Times.Once);
			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Once);
			userManager.Verify(r => r.UpdateAsync(user), Times.Once);
		}

		[TestMethod]
		public void AddFavouritedToFavourites()
		{
			var contentToFavourite = new Content
			{
				Id = 1
			};

			var favourited = new List<Content>
			{
				contentToFavourite
			};

			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username,
				Favourites = favourited
			};

			var context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			var mockContentRepository
				= new Mock<IRepository<Content>>();
			mockContentRepository
				.Setup(x => x.GetById(contentToFavourite.Id))
				.Returns(contentToFavourite);

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);
			userManager.Setup(m => m.UpdateAsync(user))
				.ReturnsAsync(IdentityResult.Success);

			var userController
				= new UserController(
					mockContentRepository.Object,
					null)
				{
					ControllerContext = context.Object,
					UserManager = userManager.Object
				};
			Assert.AreEqual(1, favourited.Count);

			var actionResult = userController.AddToFavourites(contentToFavourite.Id);

			Assert.AreEqual(1, favourited.Count);

			mockContentRepository.Verify(r => r.GetById(contentToFavourite.Id), Times.Once);
			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Once);
			userManager.Verify(r => r.UpdateAsync(user), Times.Never);
		}
	}
}
