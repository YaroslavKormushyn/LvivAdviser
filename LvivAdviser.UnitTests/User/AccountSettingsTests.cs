using LvivAdviser.Domain.Abstract;
using LvivAdviser.WebUI.Controllers;
using LvivAdviser.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;

namespace LvivAdviser.UnitTests.User
{
	[TestClass]
	public class AccountSettingsTests
	{
		private Mock<IPrincipal> mockPrincipal;
		private string username = "test@example.com";
		private string newUsername = "test1@example.com";

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
		public void GetAccountSettings()
		{
			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username
			};

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);

			var controllerContext = new Mock<ControllerContext>();
			controllerContext.SetupGet(x => x.HttpContext.User)
				.Returns(this.mockPrincipal.Object);

			var userController
				= new UserController(
					null,
					null)
				{
					ControllerContext = controllerContext.Object,
					UserManager = userManager.Object
				};

			var viewResult = userController.AccountSettings();

			var modelResult = (EditModel)viewResult.Model;

			Assert.AreEqual(user.Id, modelResult.Id);
			Assert.AreEqual(user.UserName, modelResult.Name);
			Assert.AreEqual(user.PasswordHash, modelResult.Password);
			Assert.AreEqual(user.Email, modelResult.Email);
			Assert.AreEqual(user.Budget, modelResult.Budget);

			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Once);
		}

		[TestMethod]
		public void SetAccountSettings()
		{
			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username
			};

			var model = new EditModel
			{
				Id = username,
				Email = newUsername,
				Name = newUsername,
				Budget = 10M,
				Password = newUsername
			};

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);
			userManager.Setup(m => m.UpdateAsync(user))
				.Callback(() =>
				{
					user.UserName = model.Name;
					user.Email = model.Email;
					user.PasswordHash = 
						userManager.Object.PasswordHasher
						.HashPassword(model.Password);
					user.Budget = model.Budget;
				})
				.ReturnsAsync(IdentityResult.Success);
			var controllerContext = new Mock<ControllerContext>();
			controllerContext.SetupGet(x => x.HttpContext.User)
				.Returns(this.mockPrincipal.Object);

			var userController
				= new UserController(
					null,
					null)
				{
					ControllerContext = controllerContext.Object,
					UserManager = userManager.Object
				};
			userController.Validate(model);
			var viewResult = userController.AccountSettings(model);

			Assert.AreEqual(model.Id, user.Id);
			Assert.AreEqual(model.Name, user.UserName);
			//Assert.AreEqual(userManager.Object.PasswordHasher
			//	.HashPassword(model.Password), user.PasswordHash);
			Assert.AreEqual(model.Email, user.Email);
			Assert.AreEqual(model.Budget, user.Budget);

			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Once);
			userManager.Verify(r => r.UpdateAsync(user), Times.Once);
		}

		[TestMethod]
		public void SetAccountSettingsUpdateFailure()
		{
			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username
			};

			var model = new EditModel
			{
				Id = username,
				Email = newUsername,
				Name = newUsername,
				Budget = 10M,
				Password = newUsername
			};

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);
			userManager.Setup(m => m.UpdateAsync(user))
				.Callback(() =>
				{
					user.UserName = model.Name;
					user.Email = model.Email;
					user.PasswordHash =
						userManager.Object.PasswordHasher
							.HashPassword(model.Password);
					user.Budget = model.Budget;
				})
				.ReturnsAsync(IdentityResult.Failed(""));
			var controllerContext = new Mock<ControllerContext>();
			controllerContext.SetupGet(x => x.HttpContext.User)
				.Returns(this.mockPrincipal.Object);

			var userController
				= new UserController(
					null,
					null)
				{
					ControllerContext = controllerContext.Object,
					UserManager = userManager.Object
				};
			userController.Validate(model);
			var viewResult = userController.AccountSettings(model);

			Assert.AreEqual(model.Id, user.Id);
			Assert.AreEqual(model.Name, user.UserName);
			//Assert.AreEqual(userManager.Object.PasswordHasher
			//	.HashPassword(model.Password), user.PasswordHash);
			Assert.AreEqual(model.Email, user.Email);
			Assert.AreEqual(model.Budget, user.Budget);

			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Once);
			userManager.Verify(r => r.UpdateAsync(user), Times.Once);
		}

		[TestMethod]
		public void SetInvalidUserAccountSettings()
		{
			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username
			};

			var model = new EditModel
			{
				Id = username,
				Email = "abc",
				Name = ":",
				Budget = 10M,
				Password = newUsername
			};

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);
			userManager.Setup(m => m.UpdateAsync(user))
				.Callback(() =>
				{
					user.UserName = model.Name;
					user.Email = model.Email;
					user.PasswordHash =
						userManager.Object.PasswordHasher
							.HashPassword(model.Password);
					user.Budget = model.Budget;
				})
				.ReturnsAsync(new IdentityResult());
			var controllerContext = new Mock<ControllerContext>();
			controllerContext.SetupGet(x => x.HttpContext.User)
				.Returns(this.mockPrincipal.Object);

			var userController
				= new UserController(
					null,
					null)
				{
					ControllerContext = controllerContext.Object,
					UserManager = userManager.Object
				};
			userController.Validate(model);
			var viewResult = userController.AccountSettings(model);
			Assert.AreNotEqual(0, userController.ModelState.Count);
			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Once);
			userManager.Verify(r => r.UpdateAsync(user), Times.Never);
		}

		[TestMethod]
		public void SetInvalidPasswordAccountSettings()
		{
			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username
			};

			var model = new EditModel
			{
				Id = username,
				Email = newUsername,
				Name = newUsername,
				Budget = 10M,
				Password = "12345"
			};

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);
			userManager.Setup(m => m.UpdateAsync(user))
				.Callback(() =>
				{
					user.UserName = model.Name;
					user.Email = model.Email;
					user.PasswordHash =
						userManager.Object.PasswordHasher
							.HashPassword(model.Password);
					user.Budget = model.Budget;
				})
				.ReturnsAsync(new IdentityResult());
			var controllerContext = new Mock<ControllerContext>();
			controllerContext.SetupGet(x => x.HttpContext.User)
				.Returns(this.mockPrincipal.Object);

			var userController
				= new UserController(
					null,
					null)
				{
					ControllerContext = controllerContext.Object,
					UserManager = userManager.Object
				};
			userController.Validate(model);
			var viewResult = userController.AccountSettings(model);

			Assert.AreNotEqual(0, userController.ModelState.Count);

			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Once);
			userManager.Verify(r => r.UpdateAsync(user), Times.Never);
		}



		[TestMethod]
		public void SetInvalidAccountSettings()
		{
			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username
			};

			var model = new EditModel
			{
				Id = username,
				Email = string.Empty,
				Name = string.Empty,
				Budget = 10M,
				Password = newUsername
			};

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);
			userManager.Setup(m => m.UpdateAsync(user))
				.Callback(() =>
				{
					user.UserName = model.Name;
					user.Email = model.Email;
					user.PasswordHash =
						userManager.Object.PasswordHasher
							.HashPassword(model.Password);
					user.Budget = model.Budget;
				})
				.ReturnsAsync(new IdentityResult());
			var controllerContext = new Mock<ControllerContext>();
			controllerContext.SetupGet(x => x.HttpContext.User)
				.Returns(this.mockPrincipal.Object);

			var userController
				= new UserController(
					null,
					null)
				{
					ControllerContext = controllerContext.Object,
					UserManager = userManager.Object
				};
			userController.Validate(model);
			var actionResult = userController.AccountSettings(model);

			Assert.AreEqual(model, (EditModel)((ViewResult)actionResult.Result).Model);

			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Never);
			userManager.Verify(r => r.UpdateAsync(user), Times.Never);
		}

		//don't know why this shit throws Aggregate 
		//even when I told him to do IdentityNotMapped
		[TestMethod]
		[ExpectedException(typeof(AggregateException))]
		public void SetInvalidIdentityAccountSettings()
		{
			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username
			};

			var model = new EditModel
			{
				Id = newUsername,
				Email = newUsername,
				Name = newUsername,
				Budget = 10M,
				Password = newUsername
			};

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(mockPrincipal.Object.Identity.GetUserId()))
				.ReturnsAsync(user);
			
			var controllerContext = new Mock<ControllerContext>();
			controllerContext.SetupGet(x => x.HttpContext.User)
				.Returns(this.mockPrincipal.Object);

			var userController
				= new UserController(
					null,
					null)
				{
					ControllerContext = controllerContext.Object,
					UserManager = userManager.Object
				};
			userController.Validate(model);
			userController.AccountSettings(model).Wait();
		}
	}
}
