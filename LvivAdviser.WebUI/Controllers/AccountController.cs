using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Users.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.returnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginModel details, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				User user = await UserManager.FindAsync(details.Name,
					details.Password);
				if (user == null)
				{
					ModelState.AddModelError("", "Invalid name or password.");
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
					return Redirect(returnUrl);
				}
			}
			ViewBag.returnUrl = returnUrl;
			return View(details);
		}

		[Authorize]
		public ActionResult Logout()
		{
			AuthManager.SignOut();
			return RedirectToAction("Index", "Home");
		}

		private IAuthenticationManager AuthManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private AppUserManager UserManager
		{
			get
			{
				return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
			}
		}
	}
}