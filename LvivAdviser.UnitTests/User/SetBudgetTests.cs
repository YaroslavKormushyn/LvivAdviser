using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using LvivAdviser.Domain.Abstract;
using LvivAdviser.WebUI.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LvivAdviser.UnitTests.User
{
	[TestClass]
	public class SetBudgetTests
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
		public void SetBudgetSuccess()
		{
			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 100M,
				PasswordHash = username
			};
			var newBudget = 10M;

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);
			userManager.Setup(m => m.UpdateAsync(user))
				.Callback(() =>
				{
					user.Budget = newBudget;
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

			var actionResult = userController.SetBudget(newBudget);

			Assert.AreEqual(newBudget, user.Budget);

			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Once);
			userManager.Verify(r => r.UpdateAsync(user), Times.Once);
		}

		[TestMethod]
		public void SetBudgetFailure()
		{
			var user = new Domain.Entities.User
			{
				Id = username,
				Email = username,
				UserName = username,
				Budget = 1100M,
				PasswordHash = username
			};
			var newBudget = 10M;

			var userManager = new Mock<AppUserManager>(
				new UserStore<Domain.Entities.User>());
			userManager.Setup(m => m.FindByIdAsync(user.Id))
				.ReturnsAsync(user);
			userManager.Setup(m => m.UpdateAsync(user))
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

			var actionResult = userController.SetBudget(newBudget);

			//Assert.AreNotEqual(newBudget, user.Budget);
			Assert.AreEqual(
				$"Cannot set budget of {newBudget}.", 
				((string[])((ViewResult)actionResult).Model).Single());

			userManager.Verify(r => r.FindByIdAsync(user.Id), Times.Once);
			userManager.Verify(r => r.UpdateAsync(user), Times.Once);
		}
	}
}
