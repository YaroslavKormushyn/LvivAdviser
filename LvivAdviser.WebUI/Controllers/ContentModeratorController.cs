using System.Diagnostics.CodeAnalysis;
using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LvivAdviser.WebUI.Controllers
{
	[Authorize(Roles = "Administrators")]
    public class ContentModeratorController : Controller
    {
	    private readonly IRepository<Content> _contentRepository;

	    public ContentModeratorController(IRepository<Content> repository)
		{
		    _contentRepository = repository;
		}

	     public ViewResult Index()
        {
            return View(_contentRepository.GetAll());
        }

        public ViewResult EditContent(int Id)
        {
            Content content = _contentRepository.GetAll().FirstOrDefault(c => c.Id == Id);
            return View(content);
        }

        [HttpPost]
        public ActionResult EditContent(Content content)
        {
            if (ModelState.IsValid)
            {
                _contentRepository.SaveContent(content);
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
            Content content = _contentRepository.DeleteContent(id);
            if (content != null)
            {
                TempData["message"] = $"{content.Name} was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}
