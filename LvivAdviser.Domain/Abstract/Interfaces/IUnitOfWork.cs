using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LvivAdviser.Domain.Abstract.Interfaces
{
	interface IUnitOfWork
	{
		IContentRepository Contents { get; }
		IRatingRepository Ratings { get; }
		IUserRepository Users { get; }
		int Complete();
	}
}
