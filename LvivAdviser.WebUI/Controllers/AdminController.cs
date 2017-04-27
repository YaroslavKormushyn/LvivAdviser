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

        public ViewResult EditContent(int Id)
        {
            Content content = repository.GetAll().FirstOrDefault(c => c.Id == Id);
            return View(content);
        }

        [HttpPost]
        public ActionResult EditContent(Content content)
        {
            if (ModelState.IsValid)
            {
                repository.SaveContent(content);
                //repository.Update(content);
				//repository.Save();
                TempData["message"] = $"{content.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(content);
            }
        }

        public ViewResult CreateContent()
        {
            return View("EditContent", new Content());
        }

        [HttpPost]
        public ActionResult DeleteContent(int id)
        {
            Content content = repository.DeleteContent(id);
            if (content != null)
            {
                TempData["message"] = $"{content.Name} was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}
