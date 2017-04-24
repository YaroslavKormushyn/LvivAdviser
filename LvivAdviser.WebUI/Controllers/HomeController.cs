using System.Web.Mvc;
using System.Collections.Generic;

namespace Users.Controllers
{
	public class HomeController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			Dictionary<string, object> data
				= new Dictionary<string, object>
				{
					{ "Placeholder", "Placeholder" }
				};
			return View(data);
		}
	}
}