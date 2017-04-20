using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LvivAdviser.Domain.Entities;
using LvivAdviser.Domain.Abstract.Interfaces;

namespace LvivAdviser.Domain.Abstract
{
	class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(DbContext context) : base(context)
		{
		}

		public AppDbContext AppDbContext
		{
			get { return Context as AppDbContext; }
		}
	}
}
