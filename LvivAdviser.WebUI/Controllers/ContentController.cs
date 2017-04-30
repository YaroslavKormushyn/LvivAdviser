using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Models;

using System.Linq;
using System.Web.Mvc;

namespace LvivAdviser.WebUI.Controllers
{
	public class ContentController : Controller
    {
		private IRepository<Content> _repository;
	    public int PageSize = 4;

	    public ContentController(IRepository<Content> contentRepository)
	    {
		    _repository = contentRepository;
	    }

		public ActionResult Index()
        {
            return RedirectToActionPermanent(nameof(ViewContent));
        }
        
        public ViewResult ViewContent(string type, int page = 1)
        {
            ContentListViewModel model = new ContentListViewModel
            {
                Contents = _repository.GetAll()
					.Where(p => type == null || p.Type.ToString() == type)
					.OrderBy(p => p.Id)
					.Skip((page - 1) * PageSize)
					.Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = type == null 
					? _repository.GetAll().Count() 
					: _repository
						.GetAll()
						.Count(e => e.Type.ToString() == type)
                },
                CurrentType = type
            };
            return View(model);
        }
    }
}
