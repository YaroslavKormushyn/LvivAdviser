using LvivAdviser.Domain.Abstract.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace LvivAdviser.Domain.Abstract
{
	public class AppDbRepository<T> : IRepository<T>
		where T : class 
	{
		private readonly AppDbContext context;
		private DbSet<T> table;
		private bool disposedValue = false;

		public AppDbRepository(AppDbContext context)
		{
			this.context = context;
			this.table = context.Set<T>();
		}

		public void Add(T entity)
		{
			this.table.Add(entity);
		}

		public void AddRange(IEnumerable<T> entities)
		{
			this.table.AddRange(entities);
		}

		public int Complete()
		{
			return context.SaveChanges();
		}

		public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
		{
			return this.table.Where(predicate);
		}

		public T GetById(int id) => this.table.Find(id);

		public IQueryable<T> GetAll() => this.table;

		public void Remove(T entity)
		{
			this.table.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			this.table.RemoveRange(entities);
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					this.Complete();
				}

				this.disposedValue = true;
			}
		}
	}
}
