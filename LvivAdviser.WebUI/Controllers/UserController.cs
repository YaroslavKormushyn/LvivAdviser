using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LvivAdviser.WebUI.Controllers
{
	[Authorize(Roles = "Users")]
    public class UserController : Controller
	{
		private IRepository<Content> _content;
		private IRepository<Rating> _ratings;

		public UserController(
			IRepository<Content> content, 
			IRepository<Rating> ratings)
		{
			_content = content;
			_ratings = ratings;
		}

		private AppUserManager UserManager 
			=> HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

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

			currUser.Ratings.AsQueryable().Load();
			currUser.Favourites.AsQueryable().Load();

			return View(new EditModel
			{
				Email = currUser.Email,
				Name = currUser.UserName,
				Password = currUser.PasswordHash
			});
		}
    }
}