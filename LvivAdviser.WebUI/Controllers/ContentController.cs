using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Models;

using System.Linq;
using System.Web.Mvc;

namespace LvivAdviser.WebUI.Controllers
{
	public class ContentController : Controller
    {
        // GET: Content
        public ActionResult Index()
        {
            return View();
        }
        private IRepository<Content> repository;
        public int PageSize = 4;

        public ContentController(IRepository<Content> contentRepository)
        {
            this.repository = contentRepository;
        }

        public ViewResult List(string type, int page = 1)
        {
            ContentListViewModel model = new ContentListViewModel
            {
                Contents = repository.GetAll()
					.Where(p => type == null || p.Type.ToString() == type)
					.OrderBy(p => p.ID)
					.Skip((page - 1) * PageSize)
					.Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = type == null 
					? repository.GetAll().Count() 
					: repository.GetAll()
						.Where(e => e.Type.ToString() == type)
						.Count()
                },
                CurrentType = type
            };
            return View(model);
        }
    }
}