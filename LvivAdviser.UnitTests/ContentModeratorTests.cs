using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LvivAdviser.UnitTests.Moderator
{
    [TestClass]
    public class ContentModeratorTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            Mock<IRepository<Content>> mockContentRepository = new Mock<IRepository<Content>>();
            Content[] arrayContent = new Content[]
            {
                new Content { Id = 1, Name = "c1" },
                new Content { Id = 2, Name = "c2" },
                new Content { Id = 3, Name = "c3" },
                new Content { Id = 4, Name = "c4" },
                new Content { Id = 5, Name = "c5" }
            };
            mockContentRepository.Setup(x => x.GetAll()).Returns(arrayContent.AsQueryable());
            ContentModeratorController target = new ContentModeratorController(mockContentRepository.Object);
            Content[] result = ((IEnumerable<Content>)target.Index().
            ViewData.Model).ToArray();
     
            Assert.AreEqual(result.Length, 5);
            Assert.AreEqual("c1", result[0].Name);
            Assert.AreEqual("c2", result[1].Name);
            Assert.AreEqual("c3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            Mock<IRepository<Content>> mockContentRepository = new Mock<IRepository<Content>>();
            Content[] arrayContent = new Content[]
            {
                new Content { Id = 1, Name = "c1" },
                new Content { Id = 2, Name = "c2" },
                new Content { Id = 3, Name = "c3" },
                new Content { Id = 4, Name = "c4" },
                new Content { Id = 5, Name = "c5" }
            };
            mockContentRepository.Setup(x => x.GetAll()).Returns(arrayContent.AsQueryable());
            ContentModeratorController target = new ContentModeratorController(mockContentRepository.Object);

            Content content1 = target.EditContent(1).ViewData.Model as Content;
            Content content2 = target.EditContent(2).ViewData.Model as Content;
            Content content3 = target.EditContent(3).ViewData.Model as Content;

            Assert.AreEqual(1, content1.Id);
            Assert.AreEqual(2, content2.Id);
            Assert.AreEqual(3, content3.Id);
        }
        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            Mock<IRepository<Content>> mockContentRepository = new Mock<IRepository<Content>>();
            Content[] arrayContent = new Content[]
            {
                new Content { Id = 1, Name = "c1" },
                new Content { Id = 2, Name = "c2" },
                new Content { Id = 3, Name = "c3" },
                new Content { Id = 4, Name = "c4" },
                new Content { Id = 5, Name = "c5" }
            };
            mockContentRepository.Setup(x => x.GetAll()).Returns(arrayContent.AsQueryable());
            ContentModeratorController target = new ContentModeratorController(mockContentRepository.Object);
            // Act
            Content result = (Content)target.EditContent(6).ViewData.Model;
            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IRepository<Content>> mockContentRepository = new Mock<IRepository<Content>>();
            ContentModeratorController target = new ContentModeratorController(mockContentRepository.Object);
            Content content = new Content { Name = "Test" };
            ActionResult result = target.EditContent(content);
            mockContentRepository.Verify(m => m.SaveContent(content));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IRepository<Content>> mockContentRepository = new Mock<IRepository<Content>>();
            ContentModeratorController target = new ContentModeratorController(mockContentRepository.Object);
            Content content = new Content { Name = "Test" };
            target.ModelState.AddModelError("error", "error");
            ActionResult result = target.EditContent(content);
            mockContentRepository.Verify(m => m.SaveContent(It.IsAny<Content>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            Content cont = new Content { Id = 2, Name = "Test" };
            Mock<IRepository<Content>> mockContentRepository = new Mock<IRepository<Content>>();
            Content[] arrayContent = new Content[]
           {
                new Content { Id = 1, Name = "c1" },
                cont,
                new Content { Id = 3, Name = "c3" },
           };
            mockContentRepository.Setup(x => x.GetAll()).Returns(arrayContent.AsQueryable());
            ContentModeratorController target = new ContentModeratorController(mockContentRepository.Object);
            target.DeleteContent(cont.Id);

            mockContentRepository.Verify(m => m.DeleteContent(cont.Id));
        }
    }
}

