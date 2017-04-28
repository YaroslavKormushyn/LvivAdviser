using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LvivAdviser.WebUI.Controllers
{
	public class NavController : Controller
    {
        private IRepository<Content> repository;

        public NavController(IRepository<Content> repo)
        {
            repository = repo;
        }

        public PartialViewResult FilterContent(string type = null)
        {
            ViewBag.SelectedType = type;
            IEnumerable<string> types = repository.GetAll()
				.Select(x => x.Type.ToString())
				.Distinct()
				.OrderBy(x => x);
            return PartialView(types);
        }
    }
}
