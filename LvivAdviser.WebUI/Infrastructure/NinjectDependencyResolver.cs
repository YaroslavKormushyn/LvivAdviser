using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Abstract;
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
			this.kernel.Bind<IRepository<Content>>()
				.To<AppDbRepository<Content>>();

			this.kernel.Bind<IRepository<Rating>>()
				.To<AppDbRepository<Rating>>();
		}
	}
}