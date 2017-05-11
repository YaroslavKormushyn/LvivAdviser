using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
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
	public class AddCommentTests
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
		public void AddComment()
		{
			var content = new Content
			{
				Id = 1
			};

			var model = new CommentEditModel
			{
				Comment = null,
				CommentId = 1,
				UserName = username
			};

			var ratingsList = new List<Rating>
			{
				new Rating
				{
					Id = 1,
					ContentId = content.Id,
					Rate = 2,
					UserId = username
				}
			};

			Mock<ControllerContext> context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			Mock<IRepository<Rating>> mockRatingRepository
				= new Mock<IRepository<Rating>>();
			mockRatingRepository
				.Setup(x => x.GetById(content.Id))
				.Returns(ratingsList.First);
			
			var userController
				= new UserController(
					null,
					mockRatingRepository.Object)
				{
					ControllerContext = context.Object
				};
			var viewResult = userController.AddComment(content.Id);

			Assert.AreEqual(
				model.Comment, 
				((CommentEditModel)viewResult.Model).Comment);
			Assert.AreEqual(
				model.UserName, 
				((CommentEditModel)viewResult.Model).UserName);
			Assert.AreEqual(
				model.CommentId, 
				((CommentEditModel)viewResult.Model).CommentId);

			mockRatingRepository.Verify(r => r.GetById(content.Id), Times.Once);
		}

		[TestMethod]
		public void AddInvalidIdentityComment()
		{
			var content = new Content
			{
				Id = 1
			};

			var model = new CommentEditModel
			{
				Comment = null,
				CommentId = 1,
				UserName = username
			};

			var ratingsList = new List<Rating>
			{
				new Rating
				{
					ContentId = 1,
					Rate = 2,
					UserId = otherUsername
				}
			};

			Mock<ControllerContext> context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			Mock<IRepository<Rating>> mockRatingRepository
				= new Mock<IRepository<Rating>>();
			mockRatingRepository
				.Setup(x => x.GetById(content.Id))
				.Returns(ratingsList.First);

			var userController
				= new UserController(
					null,
					mockRatingRepository.Object)
				{
					ControllerContext = context.Object
				};

			var viewResult = userController.AddComment(content.Id);
			
			Assert.AreEqual(
				"Cannot add comments from others.",
				((string[])viewResult.Model).Single());

			mockRatingRepository.Verify(r => r.GetById(content.Id), Times.Once);
		}

		[TestMethod]
		public void UpdateComment()
		{
			var content = new Content
			{
				Id = 1
			};

			var model = new CommentEditModel
			{
				Comment = "123",
				CommentId = 1,
				UserName = username
			};

			var ratingsList = new List<Rating>
			{
				new Rating
				{
					ContentId = 1,
					Rate = 2,
					UserId = username
				}
			};

			Mock<ControllerContext> context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			Mock<IRepository<Rating>> mockRatingRepository
				= new Mock<IRepository<Rating>>();
			mockRatingRepository
				.Setup(x => x.GetById(content.Id))
				.Returns(ratingsList.First(r => r.ContentId == content.Id));
			mockRatingRepository
				.Setup(r => r.Update(
					It.Is<Rating>(
						rating => rating.ContentId == model.CommentId
						          && rating.UserId == model.UserName)))
				.Callback(() =>
				{
					var r = ratingsList
						.Find(rating => rating.ContentId == model.CommentId
										&& rating.UserId == model.UserName);
					r.Comment = model.Comment;
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

			var actionResult = userController.AddComment(model);

			Assert.AreEqual(model.Comment, ratingsList.Single().Comment);

			mockRatingRepository.Verify(r => r.GetById(content.Id), Times.Once);
			mockRatingRepository.Verify(r => r.Update(It.IsAny<Rating>()), Times.Once);
			mockRatingRepository.Verify(r => r.Save(), Times.Once);
		}

		[TestMethod]
		public void UpdateInvalidIdentityComment()
		{
			var content = new Content
			{
				Id = 1
			};

			var model = new CommentEditModel
			{
				Comment = "123",
				CommentId = 1,
				UserName = username
			};

			var ratingsList = new List<Rating>
			{
				new Rating
				{
					ContentId = 1,
					Rate = 2,
					UserId = otherUsername
				}
			};

			Mock<ControllerContext> context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			Mock<IRepository<Rating>> mockRatingRepository
				= new Mock<IRepository<Rating>>();
			mockRatingRepository
				.Setup(x => x.GetById(content.Id))
				.Returns(ratingsList.First(r => r.ContentId == content.Id));
			mockRatingRepository
				.Setup(r => r.Update(
					It.Is<Rating>(
						rating => rating.ContentId == model.CommentId
						          && rating.UserId == model.UserName)))
				.Callback(() =>
				{
					var r = ratingsList
						.Find(rating => rating.ContentId == model.CommentId
						                && rating.UserId == model.UserName);
					r.Comment = model.Comment;
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

			var actionResult = userController.AddComment(model);

			Assert.AreEqual(null, ratingsList.Single().Comment);

			mockRatingRepository.Verify(r => r.GetById(content.Id), Times.Once);
			mockRatingRepository.Verify(r => r.Update(It.IsAny<Rating>()), Times.Never);
			mockRatingRepository.Verify(r => r.Save(), Times.Never);
		}
	}
}
