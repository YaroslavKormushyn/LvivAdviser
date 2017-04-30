using LvivAdviser.Domain.Abstract;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;

using Ninject;

using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject.Web.Common;

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
			//kernel.Bind<AppDbContext>()
			//	.ToSelf()
			//	.InRequestScope();

			kernel.Bind<IRepository<Content>>()
				.To<AppDbRepository<Content>>();;
				//.InRequestScope();

			kernel.Bind<IRepository<Rating>>()
				.To<AppDbRepository<Rating>>();
			//.InRequestScope();
		}
	}
}