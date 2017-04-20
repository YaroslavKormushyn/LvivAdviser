using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Ninject;
using Moq;
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
            mock.Setup(m => m.Contents).Returns(new List<Content> {
                    new Content { ID = 1, Name = "Black cat", Type = LvivAdviser.Domain.Entities.Type.Food ,Description ="restaurant", MainPhoto=null, Rating = 5},
                    new Content { ID = 2, Name = "Celentano", Type = LvivAdviser.Domain.Entities.Type.Food ,Description ="restaurant", MainPhoto=null, Rating = 4},
                    new Content { ID = 3, Name = "Da Vinci", Type = LvivAdviser.Domain.Entities.Type.Food ,Description ="restaurant/pizza", MainPhoto=null, Rating = 4},
                    new Content { ID = 4, Name = "Kredense", Type = LvivAdviser.Domain.Entities.Type.Food ,Description ="cafe", MainPhoto=null, Rating = 3},

});
            kernel.Bind<IContentRepository>().ToConstant(mock.Object);
        }
	}
}