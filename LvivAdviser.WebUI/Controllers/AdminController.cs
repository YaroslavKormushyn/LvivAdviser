using System.Linq;
using System.Web.Mvc;
using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Entities;
using LvivAdviser.Domain.Abstract.Interfaces;

namespace LvivAdviser.WebUI.Controllers
{

    public class AdminController : Controller
    {
        private IContentRepository repository;
        public AdminController(IContentRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Contents);
        }
        public ViewResult Edit(int ID)
        {
            Content content = repository.Contents.FirstOrDefault(c => c.ID == ID);
            return View(content);
        }

    }
}