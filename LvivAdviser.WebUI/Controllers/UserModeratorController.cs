using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LvivAdviser.WebUI.Controllers
{  
    [Authorize(Roles = "UserModerators")]public class UserModeratorController : Controller
    {
        private readonly IRepository<Rating> _ratingRepository;
        private readonly IRepository<Blacklist> _blacklistRepository;
        public UserModeratorController(
            IRepository<Rating> r_repository,
            IRepository<Blacklist> bl_repository)
        {
            _ratingRepository = r_repository;
            _blacklistRepository = bl_repository;
        }

        private AppUserManager UserManager
            => HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

        //[ExcludeFromCodeCoverage]
        //public ActionResult Index()
        //{
        //	return View();
        //}

        [HttpGet]
        public ViewResult ViewComment(int id)
        {
            var comment = _ratingRepository.GetById(id);

            if (comment == null)
            {
                return View("_Error", new[] { "Comment Not Found" });
            }

            return View(comment);
        }

        [HttpGet]
        public ViewResult ViewAllComments()
        {
            var comments = _ratingRepository.GetAll();

            return View(comments);
        }

        [HttpGet]
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
                return View("_Error", new[] { "Comment not found." });
            }
            return View(model);
        }
        
        public ActionResult ViewBlacklist()
        {
            var usersInBlackList = _blacklistRepository.GetAll();

            return View(usersInBlackList);
        }
        
        public async Task<ActionResult> AddUserToBlacklist()
        {
            string[] idsInBlacklist = _blacklistRepository.GetAll()
                .Select(bl => bl.UserId).ToArray();
            IEnumerable<User> notInBlacklist = UserManager.Users
                .Where(x => idsInBlacklist.All(y => y != x.Id));
            DateTime time = DateTime.Now;
            return View(new AddToBlacklistModel
            {
                DateStart = time,
                DateEnd = time.Add(new TimeSpan(1, 0, 0)),
                NotInBlacklist = notInBlacklist
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddUserToBlacklist(AddToBlacklistModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(model.UserId);
            if(user == null)
            {
                return View("_Error", new[] { "User Not Found" });
            }
                   
            _blacklistRepository.Add(new Blacklist
            {
                DateStart = model.DateStart,
                DateEnd = model.DateEnd,
                Reason = model.Reason,
                UserId = model.UserId,
                User = await UserManager.FindByIdAsync(model.UserId)
            });

            await _blacklistRepository.SaveAsync();
            return RedirectToAction("ViewBlacklist", "UserModerator");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Blacklist blacklist = _blacklistRepository.GetById(id);
            if (blacklist != null)
            {
                _blacklistRepository.Delete(blacklist);
                await _blacklistRepository.SaveAsync();
                return RedirectToAction("ViewBlacklist");
            }
            else
            {
                return View("_Error", new[] { "Not Found" });
            }
        }
    }
}