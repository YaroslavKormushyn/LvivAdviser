using LvivAdviser.Domain.Abstract.Interfaces;
using System;

namespace LvivAdviser.Domain.Abstract
{
	class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly AppDbContext context;
		private IContentRepository contents;
		private IRatingRepository ratings;
		private IUserRepository users;

		public UnitOfWork(AppDbContext context)
		{
			this.context = context;
			contents = new ContentRepository(context);
			ratings = new RatingRepository(context);
			users = new UserRepository(context);
		}

		public IContentRepository Contents
		{
			get { return contents; }
		}

		public IRatingRepository Ratings
		{
			get { return ratings; }
		}

		public IUserRepository Users
		{
			get { return users; }
		}

		public int Complete()
		{
			return context.SaveChanges();
		}

		public void Dispose()
		{
			context.Dispose();
		}
	}
}
