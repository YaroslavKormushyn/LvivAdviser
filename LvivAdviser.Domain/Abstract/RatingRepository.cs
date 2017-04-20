using LvivAdviser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using LvivAdviser.Domain.Abstract.Interfaces;

namespace LvivAdviser.Domain.Abstract
{
	class RatingRepository : Repository<Rating>, IRatingRepository
	{
		public RatingRepository(AppDbContext context) : base(context)
		{
		}

		public AppDbContext AppDbContext
		{
			get { return Context as AppDbContext; }
		}
	}
}
