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
	[Authorize(Roles = "Moderators")]
    public class ModeratorController : Controller
    {
	    private readonly IRepository<Rating> ratingRepository;

	    public ModeratorController(IRepository<Rating> repository)
	    {
			ratingRepository = repository;
	    }

	    private AppUserManager UserManager
		    => HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
		
	 //   [ExcludeFromCodeCoverage]
		//public ActionResult Index()
  //      {
		//	return RedirectToAction(nameof(this.ViewContent));
		//}

		[HttpGet]
	    public ViewResult ViewComments(int id)
		{
			var comments = ratingRepository
				.GetAll()
				.Where(comm => comm.Id == id);

			return View(comments);
		}

	    [HttpGet]
	    public ViewResult EditComment(int id)
	    {
		    var comment = ratingRepository.GetById(id);
			
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
	    public ActionResult EditComment(CommentEditModel model)
	    {
		    if (ModelState.IsValid)
		    {
			    var rating = ratingRepository.GetById(model.CommentId);
			    if (rating != null)
			    {
				    rating.Comment = model.Comment;
					ratingRepository.Update(rating);
				    var result = ratingRepository.Save();
				    if (result == 0)
				    {
					    return View("_Error");
				    }
			    }
			    return View("_Error", new[] {"Comment not found."});
		    }
		    return View(model);
	    }
    }
}