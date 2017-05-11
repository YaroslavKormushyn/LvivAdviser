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
	public class RemoveCommentTests
	{
		private Mock<IPrincipal> mockPrincipal;
		private string username = "test@example.com";
		private string otherUsername = "test1@example.com";

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
		public void RemoveComment()
		{
			var content = new Content
			{
				Id = 1
			};

			var ratingsList = new List<Rating>
			{
				new Rating
				{
					ContentId = 1,
					Rate = 2,
					UserId = username,
					Comment = "Comment",
					Id = 1
				}
			};

			Mock<ControllerContext> context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			Mock<IRepository<Rating>> mockRatingRepository
				= new Mock<IRepository<Rating>>();
			mockRatingRepository
				.Setup(x => x.GetById(ratingsList.First().Id))
				.Returns(ratingsList.First());
			mockRatingRepository
				.Setup(r => r.Update(
					It.Is<Rating>(
						rating => rating.ContentId == ratingsList.First().ContentId
						          && rating.UserId == ratingsList.First().UserId)))
				.Callback(() =>
				{
					var r = ratingsList
						.Find(rating => rating.ContentId == ratingsList.First().ContentId
										&& rating.UserId == ratingsList.First().UserId);
					r.Comment = null;
				});
			mockRatingRepository
				.Setup(r => r.Save())
				.Returns(1);

			var userController
				= new UserController(
					null,
					mockRatingRepository.Object)
				{
					ControllerContext = context.Object
				};

			var actionResult = 
				userController.RemoveComment(ratingsList.First().Id);

			Assert.AreEqual(null, ratingsList.Single().Comment);

			mockRatingRepository.Verify(r => r.GetById(content.Id), Times.Once);
			mockRatingRepository.Verify(r => r.Update(It.IsAny<Rating>()), Times.Once);
			mockRatingRepository.Verify(r => r.Save(), Times.Once);
		}

		[TestMethod]
		public void RemoveInvalidIdentityComment()
		{
			var content = new Content
			{
				Id = 1
			};

			var ratingsList = new List<Rating>
			{
				new Rating
				{
					ContentId = 1,
					Rate = 2,
					UserId = otherUsername,
					Comment = "Comment",
					Id = 1
				}
			};

			Mock<ControllerContext> context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			Mock<IRepository<Rating>> mockRatingRepository
				= new Mock<IRepository<Rating>>();
			mockRatingRepository
				.Setup(x => x.GetById(ratingsList.First().Id))
				.Returns(ratingsList.First());
			mockRatingRepository
				.Setup(r => r.Update(
					It.Is<Rating>(
						rating => rating.ContentId == ratingsList.First().ContentId
						          && rating.UserId == ratingsList.First().UserId)))
				.Callback(() =>
				{
					var r = ratingsList
						.Find(rating => rating.ContentId == ratingsList.First().ContentId
						                && rating.UserId == ratingsList.First().UserId);
					r.Comment = null;
				});
			mockRatingRepository
				.Setup(r => r.Save())
				.Returns(1);

			var userController
				= new UserController(
					null,
					mockRatingRepository.Object)
				{
					ControllerContext = context.Object
				};

			var actionResult = userController.RemoveComment(ratingsList.First().Id);

			Assert.AreNotEqual(null, ratingsList.Single().Comment);

			mockRatingRepository.Verify(r => r.GetById(content.Id), Times.Once);
			mockRatingRepository.Verify(r => r.Update(It.IsAny<Rating>()), Times.Never);
			mockRatingRepository.Verify(r => r.Save(), Times.Never);
		}
	}
}
