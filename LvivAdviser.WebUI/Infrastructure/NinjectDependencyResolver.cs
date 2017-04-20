using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Web.Mvc;
using Ninject;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;

namespace LvivAdviser.WebUI.Infrastructure
{
	public class NinjectDependencyResolver : IDependencyResolver
	{
		private IKernel kernel;

		public NinjectDependencyResolver(IKernel kernelParam)
		{
			kernel = kernelParam;
			AddBindings();
		}
		public object GetService(System.Type serviceType)
		{
			return kernel.TryGet(serviceType);
		}
		public IEnumerable<object> GetServices(System.Type serviceType)
		{
			return kernel.GetAll(serviceType);
		}
		private void AddBindings()
		{
            Mock<IContentRepository> mock = new Mock<IContentRepository>();
            mock.Setup(m => m.Content).Returns(new List<Content> {
                new Content { ID = 1, Type = LvivAdviser.Domain.Entities.Type.Food, Name = "Food", Description = "aaa", MainPhoto = null, Rating = 1 },
                new Content { ID = 2, Type = LvivAdviser.Domain.Entities.Type.Rest, Name = "Rest", Description = "bbb", MainPhoto = null, Rating = 2 },
                new Content { ID = 3, Type = LvivAdviser.Domain.Entities.Type.FreeTime, Name = "FreeTime", Description = "ccc", MainPhoto = null, Rating = 3 }
            });
            kernel.Bind<IContentRepository>().ToConstant(mock.Object);
        }
	}
}