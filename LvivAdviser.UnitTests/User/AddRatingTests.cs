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
		public void AddRatingForNonExistentContentGet()
		{
			var contentToRate = new Content
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
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(It.Is<Content>(null));

			var userController
				= new UserController(
					mockContentRepository.Object,
					null)
				{
					ControllerContext = context.Object
				};

			var viewResult = userController.AddRating(contentToRate.Id);
			Assert.AreEqual("_Error", viewResult.ViewName);
			Assert.AreEqual("No such content.", ((IEnumerable<string>) viewResult.Model).Single());
		}

		[TestMethod]
		public void AddExistentRating()
		{
			var contentToRate = new Content
			{
				Id = 1
			};

			var model = new RatingEditModel
			{
				Comment = "edit",
				ContentId = 1,
				Rating = 1,
				UserId = username
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

			Mock<IRepository<Content>> mockContentRepository
				= new Mock<IRepository<Content>>();
			mockContentRepository
				.Setup(x => x.GetById(contentToRate.Id))
				.Returns(contentToRate);

			Mock<IRepository<Rating>> mockRatingRepository
				= new Mock<IRepository<Rating>>();
			mockRatingRepository
				.Setup(x => x.GetAll())
				.Returns(ratingsList.AsQueryable());
			mockRatingRepository
				.Setup(r => r.Update(
					It.Is<Rating>(
						rating => rating.ContentId == model.ContentId
						          && rating.UserId == model.UserId)))
				.Callback(() =>
				{
					var r = ratingsList
						.Find(rating => rating.ContentId == model.ContentId
						                && rating.UserId == model.UserId);
					r.Comment = model.Comment;
					r.Rate = model.Rating;
				});
			mockRatingRepository
				.Setup(r => r.SaveAsync())
				.Returns(Task.FromResult(1));

			var userController
				= new UserController(
					mockContentRepository.Object,
					mockRatingRepository.Object)
				{
					ControllerContext = context.Object
				};
			Assert.AreEqual(null, ratingsList.First().Comment);
			Assert.AreEqual(2, ratingsList.First().Rate);

			var viewResult = userController.AddRating(model);

			Assert.AreEqual(1, ratingsList.Count);
			Assert.AreEqual(model.Comment, ratingsList.First().Comment);
			Assert.AreEqual(model.Rating, ratingsList.First().Rate);

			mockContentRepository.Verify(
				r => r.GetById(contentToRate.Id), Times.Once);
			mockRatingRepository.Verify(
				r => r.GetAll(), Times.Exactly(2));
			mockRatingRepository.Verify(
				r => r.Update(It.IsAny<Rating>()), Times.Once);
			mockRatingRepository.Verify(
				r => r.Add(It.IsAny<Rating>()), Times.Never);
			mockRatingRepository.Verify(
				r => r.SaveAsync(), Times.Once);
		}

		[TestMethod]
		public void AddNonExistentRating()
		{
			var contentToRate = new Content
			{
				Id = 1
			};

			var model = new RatingEditModel
			{
				Comment = "",
				ContentId = 1,
				Rating = 1,
				UserId = username
			};

			var ratingsList = new List<Rating>
			{
				new Rating
				{
					ContentId = 2,
					UserId = username
				}
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

			Mock<IRepository<Rating>> mockRatingRepository
				= new Mock<IRepository<Rating>>();
			mockRatingRepository
				.Setup(x => x.GetAll())
				.Returns(ratingsList.AsQueryable());
			mockRatingRepository
				.Setup(r => r.Add(
					It.Is<Rating>(
						rating => rating.ContentId == model.ContentId
									&& rating.UserId == model.UserId)))
				.Callback(() => ratingsList.Add(new Rating()));
			mockRatingRepository
				.Setup(r => r.SaveAsync())
				.Returns(Task.FromResult(1));

			var userController
				= new UserController(
					mockContentRepository.Object,
					mockRatingRepository.Object)
				{
					ControllerContext = context.Object
				};

			var viewResult = userController.AddRating(model);

			Assert.AreEqual(2, ratingsList.Count);
			
			mockContentRepository.Verify(
				r => r.GetById(contentToRate.Id), Times.Once);
			mockRatingRepository.Verify(
				r => r.GetAll(), Times.Once);
			mockRatingRepository.Verify(
				r => r.Update(It.IsAny<Rating>()), Times.Never);
			mockRatingRepository.Verify(
				r => r.Add(It.IsAny<Rating>()), Times.Once);
			mockRatingRepository.Verify(
				r => r.SaveAsync(), Times.Once);
		}

		[TestMethod]
		public void AddRatingForNonExistentContent()
		{
			var contentToRate = new Content
			{
				Id = 1
			};

			var model = new RatingEditModel
			{
				Comment = "",
				ContentId = 1,
				Rating = 1,
				UserId = username
			};

			var ratingsList = new List<Rating>
			{
				new Rating
				{
					ContentId = 2,
					UserId = username
				}
			};

			Mock<ControllerContext> context
				= new Mock<ControllerContext>();
			context.SetupGet(x => x.HttpContext.User)
				.Returns(mockPrincipal.Object);

			Mock<IRepository<Content>> mockContentRepository
				= new Mock<IRepository<Content>>();
			mockContentRepository
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(It.Is<Content>(null));

			var userController
				= new UserController(
					mockContentRepository.Object,
					null)
				{
					ControllerContext = context.Object
				};

			var viewResult = userController.AddRating(model);

			Assert.AreEqual(1, ratingsList.Count);

			mockContentRepository.Verify(
				r => r.GetById(contentToRate.Id), Times.Once);
		}

		[TestMethod]
		public void AddRatingInvalidModelState()
		{
			var model = new RatingEditModel
			{
				Rating = 1
			};

			var userController
				= new UserController(
					null,
					null);
			userController.Validate(model);
			var viewResult = userController.AddRating(model);
		}
	}
}
