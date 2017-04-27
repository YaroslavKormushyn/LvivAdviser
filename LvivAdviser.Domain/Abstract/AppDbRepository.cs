using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
			table.Attach(entity);
			context.Entry(entity).State = EntityState.Modified;
		}

		public void Delete(int id)
		{
			T dbEntry = table.Find(id);
			if (dbEntry != null)
			{
				table.Remove(dbEntry);
			}
			context.Entry(new T { Id = id }).State
				= EntityState.Deleted;
		}

		public void Delete(T entity)
		{
			DbEntityEntry dbEntityEntry = context.Entry(entity);
			if (dbEntityEntry.State != EntityState.Detached)
			{
				dbEntityEntry.State = EntityState.Deleted;
			}
			else
			{
				table.Attach(entity);
				table.Remove(entity);
			}
			context.Entry(entity).State = EntityState.Deleted;
		}

		public int Save()
		{
			return context.SaveChanges();
		}

		public void SaveContent(Content content)
		{
			if (content.Id == 0)
			{
				context.Contents.Add(content);
			}
			else
			{
				Content dbEntry = context.Contents.Find(content.Id);
				if (dbEntry != null)
				{
					dbEntry.Type = content.Type;
					dbEntry.Name = content.Name;
					dbEntry.Description = content.Description;
					dbEntry.MainPhoto = content.MainPhoto;
					dbEntry.Ratings = content.Ratings;
				}
				
			}
			context.SaveChanges();
		}

		public Content DeleteContent(int Id)
		{
			Content dbEntry = context.Contents.Find(Id);
			if (dbEntry != null)
			{
				context.Contents.Remove(dbEntry);
				context.SaveChanges();
			}
			return dbEntry;
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
