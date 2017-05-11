using Castle.Core.Internal;
using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LvivAdviser.WebUI.Controllers
{
	[Authorize(Roles = "Users")]
    public class UserController : Controller
	{
		private readonly IRepository<Content> _content;
		private readonly IRepository<Rating> _ratings;
		private AppUserManager _userManager;

		public UserController(
			IRepository<Content> content, 
			IRepository<Rating> ratings)
		{
			_content = content;
			_ratings = ratings;
		}

		public AppUserManager UserManager
		{
			get => _userManager ?? 
				HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
			set => _userManager = value;
		}

		[ExcludeFromCodeCoverage]
		[HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction(nameof(AccountSettings));
        }

		[HttpGet]
		public ViewResult AccountSettings()
		{
			var currUser = 
				UserManager.FindById(User.Identity.GetUserId());
			
			return View(new EditModel
			{
				Id = currUser.Id,
				Email = currUser.Email,
				Name = currUser.UserName,
				Budget = currUser.Budget,
				Password = currUser.PasswordHash
			});
		}

		[HttpPost]
		public async Task<ActionResult> AccountSettings(EditModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var currUser =
				UserManager.FindById(User.Identity.GetUserId());

			if (currUser.Id != model.Id)
			{
				throw new IdentityNotMappedException(
					"Cannot change profile preferences for another identity.");
			}

			currUser.Budget = model.Budget;
			currUser.Email = model.Email;
			currUser.UserName = model.Name;

			var userValidation = 
				await UserManager.UserValidator.ValidateAsync(currUser);
			if (!userValidation.Succeeded)
			{
				AddErrorsFromResult(userValidation);
				return View(model);
			}

			if (!string.IsNullOrEmpty(model.Password))
			{
				var passwordValidation =
					await UserManager.PasswordValidator
						.ValidateAsync(model.Password);
				if (!passwordValidation.Succeeded)
				{
					AddErrorsFromResult(passwordValidation);
					return View(model);
				}

				currUser.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
			}

			var updateResult = UserManager.Update(currUser);
			if (updateResult.Succeeded)
			{
				TempData["Message"] = "Profile updated.";
				return RedirectToActionPermanent("AccountSettings");
			}
			return View(model);
		}

		[HttpGet]
		public ViewResult AddRating(int id)
		{
			var content = _content.GetById(id);
			if (content == null)
			{
				return View("_Error", new[] {"No such content."});
			}

			return View(new RatingEditModel
			{
				ContentId = id,
				UserId = User.Identity.GetUserId()
			});
		}

		[HttpPost]
		public async Task<ActionResult> AddRating(RatingEditModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var content = _content.GetById(model.ContentId);
			if (content == null)
			{
				return View("_Error", new[] { "No such content." });
			}

			if(_ratings.GetAll()
				.Any(r =>
					r.ContentId == model.ContentId
					&& r.UserId == User.Identity.GetUserId()))
			{
				var rating = _ratings.GetAll()
					.ToArray()
					.Find(r => r.ContentId == model.ContentId
					           && r.UserId == User.Identity.GetUserId());
				rating.Comment = model.Comment;
				rating.Rate = model.Rating;
				_ratings.Update(rating);
			}
			else
			{
				_ratings.Add(new Rating
				{
					Comment = model.Comment,
					ContentId = model.ContentId,
					UserId = model.UserId,
					Rate = model.Rating
				});
			}
			
			await _ratings.SaveAsync();
			return RedirectToAction("ViewContent", "Content");
		}

		[HttpGet]
		public ViewResult AddComment(int id)
		{
			var rating = _ratings.GetById(id);
			if (rating.UserId != User.Identity.GetUserId())
			{
				return View("_Error", new[] { "Cannot add comments from others." });
			}
			
			return View(new CommentEditModel
			{
				Comment = rating.Comment,
				CommentId = rating.Id,
				UserName = User.Identity.GetUserName()
			});
		}

		[HttpPost]
		public ActionResult AddComment(CommentEditModel model)
		{
			var rating = _ratings.GetById(model.CommentId);
			if (rating.UserId != User.Identity.GetUserId())
			{
				return View("_Error", new[] { "Cannot add comments from others." });
			}
			rating.Comment = model.Comment;
			_ratings.Update(rating);
			_ratings.Save();

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ViewResult EditComment(int id)
		{
			var rating = _ratings.GetById(id);
			if (rating.UserId != User.Identity.GetUserId())
			{
				return View("_Error", new[] { "Cannot edit comments from others." });
			}

			return View(new CommentEditModel
			{
				Comment = rating.Comment,
				CommentId = rating.Id,
				UserName = User.Identity.GetUserName()
			});
		}

		[HttpPost]
		public ActionResult EditComment(CommentEditModel model)
		{
			var rating = _ratings.GetById(model.CommentId);
			if (rating.UserId != User.Identity.GetUserId())
			{
				return View("_Error", new[] { "Cannot edit comments from others." });
			}
			rating.Comment = model.Comment;
			_ratings.Update(rating);
			_ratings.Save();

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public ActionResult RemoveComment(int id)
		{
			var rating = _ratings.GetById(id);
			if (rating.UserId != User.Identity.GetUserId())
			{
				return View("_Error", new[] {"Cannot remove comments from others."});
			}
			rating.Comment = null;
			_ratings.Update(rating);
			_ratings.Save();

			return RedirectToAction("Index", "Home");
		}

		public ActionResult AddToFavourites(int id)
		{
			var content = _content.GetById(id);
			if (content == null)
			{
				return View("_Error", new[] {"No such content."});
			}

			var currUser = UserManager.FindById(User.Identity.GetUserId());
			currUser.Favourites.AsQueryable().Load();
			if (!currUser.Favourites.Contains(content))
			{
				currUser.Favourites.Add(content);
				UserManager.Update(currUser);

				TempData["Message"] = $"{content.Name} succesfully added your favourites.";
			}
			else
			{
				TempData["Message"] = $"{content.Name} is already in your favourites.";
			}
			return RedirectToAction("Index", "Home");
		}

		public ActionResult RemoveFromFavourites(int id)
		{
			var content = _content.GetById(id);
			if (content == null)
			{
				return View("_Error", new[] { "No such content." });
			}

			var currUser = UserManager.FindById(User.Identity.GetUserId());
			if (currUser.Favourites.Contains(content))
			{
				currUser.Favourites.Remove(content);
				UserManager.Update(currUser);

				TempData["Message"] = 
					$"{content.Name} succesfully removed from your favourites.";
			}
			else
			{
				TempData["Message"] = $"{content.Name} is not in your favourites.";
			}
			return RedirectToAction("Index", "Home");
		}

		public ActionResult SetBudget(decimal budget)
		{
			var currUser = UserManager.FindById(User.Identity.GetUserId());
			currUser.Budget = budget;

			var result = UserManager.Update(currUser);
			if (!result.Succeeded)
			{
				return View("_Error", new[] {$"Cannot set budget of {budget}."});
			}
			return RedirectToAction("Index");
		}

		private void AddErrorsFromResult(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}
	}
}