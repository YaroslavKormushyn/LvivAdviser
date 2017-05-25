using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Principal;
using System.Linq;
using Moq;
using System.Security.Claims;
using LvivAdviser.WebUI.Models;
using LvivAdviser.Domain.Entities;
using System.Collections.Generic;
using System.Web.Mvc;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.WebUI.Controllers;
using LvivAdviser.Domain.Abstract;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LvivAdviser.UnitTests.UserModerator
{
    [TestClass]
    public class AddUserToBlacklistTests
    {
        private Mock<IPrincipal> mockPrincipal;
        private string username = "test@example.com";
        private string otherUsername = "test1@example.com";

        [TestMethod]
        public void AddUserToBlackList()
        {
            var start = new DateTime();

            var end = start.AddHours(1);

            var reason = "a";

            var blacklist = new List<Blacklist>
            {
                new Blacklist
                {
                    Id = 1,
                    DateStart = start,
                    DateEnd = end,
                    Reason = reason,
                    UserId = username
                }
            };

            Mock<IRepository<Blacklist>> mockBlacklistRepository
                = new Mock<IRepository<Blacklist>>();
            mockBlacklistRepository
                .Setup(x => x.GetAll())
                .Returns(blacklist.AsQueryable());

            var userManager = new Mock<AppUserManager>(
                new UserStore<Domain.Entities.User>());
            var users = new List<Domain.Entities.User>
            {
                new Domain.Entities.User{ Id = username },
                new Domain.Entities.User{ Id = otherUsername }
            };

            userManager.Setup(x => x.Users).Returns(users.AsQueryable());

            var userModeratorController
                = new UserModeratorController(
                    null,
                    mockBlacklistRepository.Object)
                {
                    UserManager = userManager.Object
                };

            var model = new AddToBlacklistModel
            {
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now.AddHours(1),
                NotInBlacklist = users.Where(x => x.Id != username)
            };

            var viewResult = userModeratorController.AddUserToBlacklist();

            Assert.AreEqual(
                model.NotInBlacklist.Count(),
                ((AddToBlacklistModel)viewResult.Model).NotInBlacklist.Count());
            Assert.AreEqual(
                model.NotInBlacklist.Single(),
                ((AddToBlacklistModel)viewResult.Model).NotInBlacklist.Single());

            mockBlacklistRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [TestMethod]
        public void AddBlacklistedUserToBlackList()
        {
            var start = new DateTime();

            var end = start.AddHours(1);

            var reason = "a";

            var blacklist = new List<Blacklist>
            {
                new Blacklist
                {
                    Id = 1,
                    DateStart = start,
                    DateEnd = end,
                    Reason = reason,
                    UserId = username
                }
            };

            Mock<IRepository<Blacklist>> mockBlacklistRepository
                = new Mock<IRepository<Blacklist>>();
            mockBlacklistRepository
                .Setup(x => x.GetAll())
                .Returns(blacklist.AsQueryable());

            var userManager = new Mock<AppUserManager>(
                new UserStore<Domain.Entities.User>());
            var users = new List<Domain.Entities.User>
            {
                new Domain.Entities.User{ Id = username },
                new Domain.Entities.User{ Id = otherUsername }
            };

            userManager.Setup(x => x.Users).Returns(users.AsQueryable());

            var userModeratorController
                = new UserModeratorController(
                    null,
                    mockBlacklistRepository.Object)
                {
                    UserManager = userManager.Object
                };



            var model = new AddToBlacklistModel
            {
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now.AddHours(1),
                NotInBlacklist = users.Where(x => x.Id != username)
            };

            var viewResult = userModeratorController.AddUserToBlacklist();

            Assert.AreEqual(
                model.NotInBlacklist.Count(),
                ((AddToBlacklistModel)viewResult.Model).NotInBlacklist.Count());
            Assert.AreEqual(
                model.NotInBlacklist.Single(),
                ((AddToBlacklistModel)viewResult.Model).NotInBlacklist.Single());

            mockBlacklistRepository.Verify(r => r.GetAll(), Times.Once);
        }
    }
}
