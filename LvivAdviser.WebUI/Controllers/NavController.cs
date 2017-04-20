using System.Collections.Generic;
using System.Web.Mvc;
using LvivAdviser.Domain.Abstract.Interfaces;
using System.Linq;

namespace LvivAdviser.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IContentRepository repository;
        public NavController(IContentRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string type = null)
        {
            ViewBag.SelectedType = type;
            IEnumerable<string> types = repository.Content.Select(x => x.Type.ToString()).Distinct().OrderBy(x => x);
            return PartialView(types);
        }
       
    }
}