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
    public class RemoveUserFromBlacklistTests
    {
        private Mock<IPrincipal> mockPrincipal;
        private string username = "test@example.com";
        private string otherUsername = "test1@example.com";

        [TestMethod]
        public void RemoveUserFromBlackList()
        {
            var start = new DateTime();

            var end = start.AddHours(1);

            var blist = new Blacklist
            {
                Id = 1,
                DateStart = start,
                DateEnd = end,
                Reason = "1",
                UserId = username
            };

            var blacklist = new List<Blacklist>
            {
                blist,
                new Blacklist
                {
                    Id = 2,
                    DateStart = start,
                    DateEnd = end,
                    Reason = "2",
                    UserId = username
                },
                new Blacklist
                {
                    Id = 3,
                    DateStart = start,
                    DateEnd = end,
                    Reason = "3",
                    UserId = username
                }
            };

            Mock<IRepository<Blacklist>> mockBlacklistRepository
                = new Mock<IRepository<Blacklist>>();
            mockBlacklistRepository
                .Setup(x => x.GetAll())
                .Returns(blacklist.AsQueryable());

            var userModeratorController
                = new UserModeratorController(
                    null,
                    mockBlacklistRepository.Object);

            var actionResult = userModeratorController.RemoveFromBlacklist(blist.Id);
            
            mockBlacklistRepository.Verify(r => r.GetById(blacklist.First().Id), Times.Once);
            mockBlacklistRepository.Verify(r => r.Delete(blist.Id), Times.Never);
        }

       
    }
}
