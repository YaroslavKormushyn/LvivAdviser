using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Controllers;
using LvivAdviser.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LvivAdviser.UnitTests.User
{
	[TestClass]
	public class AddRatingTests
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
		public void AddRatingGet()
		{
			Content contentToRate = new Content()
			{
				Id = 1
			};


			Mock<ControllerContext> context 
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			Mock<IRepository<Content>> mockContentRepository
				= new Mock<IRepository<Content>>();
			mockContentRepository
				.Setup(x => x.GetById(contentToRate.Id))
				.Returns(contentToRate);

			var userController 
				= new UserController(
					mockContentRepository.Object, 
					null)
			{
				ControllerContext = context.Object
			};

			var viewResult = userController.AddRating(contentToRate.Id);
			Assert.IsTrue(viewResult.Model is RatingEditModel);
			Assert.AreEqual(contentToRate.Id, ((RatingEditModel)viewResult.Model).ContentId);
			Assert.AreEqual(username, ((RatingEditModel)viewResult.Model).UserId);
		}
		
		[TestMethod]
		public void AddRatingNonExistentContent()
		{
			var contentToRate = new Content
			{
				Id = 1
			};

			//List<Rating> ratingsList = new List<Rating>
			//{
			//	new Rating
			//	{
			//		ContentId = 1,
			//		UserId = username
			//	}
			//};

			Mock<ControllerContext> context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			Mock<IRepository<Content>> mockContentRepository
				= new Mock<IRepository<Content>>();
			mockContentRepository
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(It.Is<Content>(null));

			//Mock<IRepository<Rating>> mockRatingsRepository
			//	= new Mock<IRepository<Rating>>();
			//mockContentRepository
			//	.Setup(x => x.GetById());

			var userController
				= new UserController(
					mockContentRepository.Object,
					null)
				{
					ControllerContext = context.Object
				};

			var viewResult = userController.AddRating(contentToRate.Id);
			//Assert.IsTrue(viewResult.Model is typeof(Array));
			//Assert.AreEqual(contentToRate.Id, ((RatingEditModel)viewResult.Model).ContentId);
			//Assert.AreEqual(username, ((RatingEditModel)viewResult.Model).UserId);
			Assert.AreEqual("_Error", viewResult.ViewName);
			Assert.AreEqual("No such content.", ((IEnumerable<string>) viewResult.Model).Single());
		}
	}
}
