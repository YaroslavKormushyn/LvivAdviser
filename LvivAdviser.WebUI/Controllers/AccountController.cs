using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Castle.Core.Internal;

using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace LvivAdviser.WebUI.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		[HttpGet]
		[AllowAnonymous]
		public ActionResult LogIn(string returnUrl)
		{
			ViewBag.returnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> LogIn(LoginModel details, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				User user = await UserManager.FindAsync(details.Name,
					details.Password);

				if (user == null)
				{
					ModelState.AddModelError("", @"Invalid name or password.");
				}
				else
				{
					ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
						DefaultAuthenticationTypes.ApplicationCookie);

					AuthManager.SignOut();
					AuthManager.SignIn(new AuthenticationProperties
					{
						IsPersistent = false
					}, ident);

					if (returnUrl.IsNullOrEmpty())
					{
						return RedirectToAction(
							nameof(HomeController.Index), "Home");
					}

					return Redirect(returnUrl);
				}
			}
			ViewBag.returnUrl = returnUrl;
			return View(details);
		}

		[Authorize]
		public ActionResult LogOut()
		{
			AuthManager.SignOut();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult SignUp()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction(
					nameof(HomeController.Index), "Home");
			}
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> SignUp(SignUpModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new User
				{
					UserName = model.Name,
					Email = model.Email
				};

				var result = await UserManager.CreateAsync(
					user, model.Password);

				if (result.Succeeded)
				{
					result = await UserManager.AddToRoleAsync(
						user.Id, "Users");

					if (result.Succeeded)
					{
						var ident = await UserManager.CreateIdentityAsync(
							user, DefaultAuthenticationTypes.ApplicationCookie);

						AuthManager.SignOut();
						AuthManager.SignIn(
							new AuthenticationProperties
							{
								IsPersistent = true
							},
							ident);

						return RedirectToAction(
							nameof(HomeController.Index), "Home");
					}

					await UserManager.DeleteAsync(user);
				}

				AddErrorsFromResult(result);
			}

			return View(model);
		}

		private void AddErrorsFromResult(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private IAuthenticationManager AuthManager 
			=> HttpContext.GetOwinContext().Authentication;

		private AppUserManager UserManager 
			=> HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
	}
}