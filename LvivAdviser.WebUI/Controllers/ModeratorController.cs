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
	[Authorize]
    public class ModeratorController : Controller
    {
	    private readonly IRepository<Rating> _ratingRepository;

	    private readonly IRepository<Content> _contentRepository;

	    public ModeratorController(IRepository<Rating> repository, IRepository<Content> repo )
		{
		    _ratingRepository = repository;
		    _contentRepository = repo;
		}

	    private AppUserManager UserManager
		    => HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

		//[ExcludeFromCodeCoverage]
		//public ActionResult Index()
		//{
		//	return View();
		//}

		[HttpGet]
		[Authorize(Roles = "UserModerators")]
	    public ViewResult ViewComments(int id)
		{
			var comments = _ratingRepository
				.GetAll()
				.Where(comm => comm.Id == id);

			return View(comments);
		}

	    [HttpGet]
		[Authorize(Roles = "UserModerators")]
	    public ViewResult EditComment(int id)
	    {
		    var comment = _ratingRepository.GetById(id);
			
		    if (comment == null)
		    {
				return View("_Error", new[] { "Comment Not Found" });
			}
			return View(new CommentEditModel
			{
				CommentId = comment.Id,
				UserName = comment.User.UserName,
				Comment = comment.Comment
			});
	    }

		[HttpPost]
		[Authorize(Roles = "UserModerators")]
	    public ActionResult EditComment(CommentEditModel model)
	    {
		    if (ModelState.IsValid)
		    {
			    var rating = _ratingRepository.GetById(model.CommentId);
			    if (rating != null)
			    {
				    rating.Comment = model.Comment;
					_ratingRepository.Update(rating);
				    var result = _ratingRepository.Save();
				    if (result == 0)
				    {
					    return View("_Error");
				    }
			    }
			    return View("_Error", new[] {"Comment not found."});
		    }
		    return View(model);
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
        [Authorize(Roles = "Administrators")]
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
        [Authorize(Roles = "Administrators")]
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
