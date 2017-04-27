using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LvivAdviser.Domain.Abstract.Interfaces;
using LvivAdviser.Domain.Entities;

namespace LvivAdviser.Domain.Abstract
{
	[ExcludeFromCodeCoverage]
	public class AppDbRepository<T> : IRepository<T>
		where T : EntityBase, new()
	{
		private readonly AppDbContext context;
		private bool disposedValue;
		private readonly DbSet<T> table;

		public AppDbRepository(AppDbContext context)
		{
			this.context = context;
			table = context.Set<T>();
		}

		public void Add(T entity)
		{
			table.Add(entity);
		}

		public void AddRange(IEnumerable<T> entities)
		{
			table.AddRange(entities);
		}

		public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
		{
			return table.Where(predicate);
		}

		public T GetById(int id)
		{
			return table.Find(id);
		}

		public IQueryable<T> GetAll()
		{
			return table;
		}

		public void Remove(T entity)
		{
			table.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			table.RemoveRange(entities);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public void Update(T entity)
		{
			context.Entry(entity).State = EntityState.Modified;
		}

		public void Delete(int id)
		{
			context.Entry(new T {Id = id}).State
				= EntityState.Deleted;
		}

		public void Delete(T entity)
		{
			context.Entry(entity).State = EntityState.Deleted;
		}

		public int Save()
		{
			return context.SaveChanges();
		}

		public Task<int> SaveAsync()
		{
			return context.SaveChangesAsync();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
					Save();

				disposedValue = true;
			}
		}
	}
}