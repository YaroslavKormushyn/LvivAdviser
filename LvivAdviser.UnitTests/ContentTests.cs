using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Controllers;
using LvivAdviser.WebUI.Models;
using LvivAdviser.WebUI.HtmlHelpers;

namespace LvivAdviser.UnitTests
{
    [TestClass]
    public class ContentTests
    {
        [TestMethod]
        public void Can_Paginate()
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

            mockContentRepository
                .Setup(x => x.GetAll())
                .Returns(arrayContent.AsQueryable());

            ContentController controller = new ContentController(mockContentRepository.Object);
            controller.PageSize = 3;
            ContentListViewModel result = (ContentListViewModel)controller.ViewContent(null, 2).Model;

            Content[] array = result.Contents.ToArray();
            Assert.IsTrue(array.Length == 2);
            Assert.AreEqual(array[0].Name, "c4");
            Assert.AreEqual(array[1].Name, "c5");
        }
        
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
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

            mockContentRepository
               .Setup(x => x.GetAll())
               .Returns(arrayContent.AsQueryable());

            ContentController controller = new ContentController(mockContentRepository.Object);
            controller.PageSize = 3;
            ContentListViewModel result = (ContentListViewModel)controller.ViewContent(null, 2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
            + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
            result.ToString());
        }
     
    }
}
