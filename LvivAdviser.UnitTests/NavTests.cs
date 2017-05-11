using static LvivAdviser.Domain.Entities.Type;

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;
using LvivAdviser.WebUI.Controllers;
using LvivAdviser.WebUI.Models;

namespace LvivAdviser.UnitTests
{
    [TestClass]
    public class NavTests
    {
        [TestMethod]
        public void Can_Filter_Content()
        {
            Mock<IRepository<Content>> mockContentRepository = new Mock<IRepository<Content>>();
            Content[] arrayContent = new Content[]
            {
                new Content { Id = 1, Name = "c1", Type = Food },
                new Content { Id = 2, Name = "c2", Type = Rest },
                new Content { Id = 3, Name = "c3", Type = Food },
                new Content { Id = 4, Name = "c4", Type = Rest },
                new Content { Id = 5, Name = "c5", Type = FreeTime }
            };

            mockContentRepository
              .Setup(x => x.GetAll())
              .Returns(arrayContent.AsQueryable());

            ContentController controller = new ContentController(mockContentRepository.Object);
            controller.PageSize = 3;
            ContentListViewModel result = (ContentListViewModel)controller.ViewContent("Rest", 1).Model;

            Content[] array = result.Contents.ToArray();
            Assert.AreEqual(array.Length, 2);
            Assert.IsTrue(array[0].Name == "c2" && array[0].Type == Rest);
            Assert.IsTrue(array[1].Name == "c4" && array[1].Type == Rest);
        }

        [TestMethod]
        public void Can_Create_Types()
        {
            Mock<IRepository<Content>> mockContentRepository = new Mock<IRepository<Content>>();
            Content[] arrayContent = new Content[]
             {
                new Content { Id = 1, Name = "c1", Type = Food},
                new Content { Id = 2, Name = "c2", Type = Food },
                new Content { Id = 3, Name = "c3", Type = Rest },
                new Content { Id = 4, Name = "c4", Type = FreeTime }
             };

            mockContentRepository
              .Setup(x => x.GetAll())
              .Returns(arrayContent.AsQueryable());

            NavController controller = new NavController(mockContentRepository.Object);
            string[] results = ((IEnumerable<string>)controller.FilterContent().Model).ToArray();

            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], Food.ToString());
            Assert.AreEqual(results[1], FreeTime.ToString());
            Assert.AreEqual(results[2], Rest.ToString());
        }

         [TestMethod]
        public void Indicates_Selected_Type()
        {
            Mock<IRepository<Content>> mockContentRepository = new Mock<IRepository<Content>>();
            Content[] arrayContent = new Content[]
             {
                new Content { Id = 1, Name = "c1", Type = Food },
                new Content { Id = 2, Name = "c2", Type = FreeTime }
             };

            mockContentRepository
              .Setup(x => x.GetAll())
              .Returns(arrayContent.AsQueryable());

            NavController controller = new NavController(mockContentRepository.Object);
            string typeToSelect = Food.ToString();
            string result = controller.FilterContent(typeToSelect).ViewBag.SelectedType;

            Assert.AreEqual(typeToSelect, result);
         }

        [TestMethod]
        public void Generate_Type_Specific_Content_Count()
        {
            Mock<IRepository<Content>> mockContentRepository = new Mock<IRepository<Content>>();
            Content[] arrayContent = new Content[]
            {
                new Content { Id = 1, Name = "c1", Type = Food },
                new Content { Id = 2, Name = "c2", Type = Rest },
                new Content { Id = 3, Name = "c3", Type = Food },
                new Content { Id = 4, Name = "c4", Type = Rest },
                new Content { Id = 5, Name = "c5", Type = FreeTime }
            };

            mockContentRepository
              .Setup(x => x.GetAll())
              .Returns(arrayContent.AsQueryable());

            ContentController controller = new ContentController(mockContentRepository.Object);
            controller.PageSize = 3;

            int res1 = ((ContentListViewModel)controller.ViewContent("Food", 1).Model).PagingInfo.TotalItems;
            int res2 = ((ContentListViewModel)controller.ViewContent("Rest", 1).Model).PagingInfo.TotalItems;
            int res3 = ((ContentListViewModel)controller.ViewContent("FreeTime", 1).Model).PagingInfo.TotalItems;
            int resAll = ((ContentListViewModel)controller.ViewContent(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
