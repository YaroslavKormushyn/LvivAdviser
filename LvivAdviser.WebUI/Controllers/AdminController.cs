using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;

using System.Linq;
using System.Web.Mvc;

namespace LvivAdviser.WebUI.Controllers
{
	[Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        private IRepository<Content> repository;

		public AdminController(IRepository<Content> repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.GetAll());
        }

        public ViewResult Edit(int id)
        {
            Content content = repository.GetAll().FirstOrDefault(c => c.Id == id);
            return View(content);
        }

        [HttpPost]
        public ActionResult Edit(Content content)
        {
            if (ModelState.IsValid)
            {
                repository.Update(content);
                TempData["message"] = string.Format("{0} has been saved",
					content.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(content);
            }
        }

    }
}