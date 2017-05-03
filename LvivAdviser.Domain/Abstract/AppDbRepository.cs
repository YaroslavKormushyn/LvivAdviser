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
		private readonly AppDbContext _context;
		private bool _disposedValue;
		private readonly DbSet<T> _table;

		public AppDbRepository(AppDbContext context)
		{
			_context = context;
			_table = context.Set<T>();
		}

		public void Add(T entity)
		{
			_table.Add(entity);
		}

		public void AddRange(IEnumerable<T> entities)
		{
			_table.AddRange(entities);
		}

		public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
		{
			return _table.Where(predicate);
		}

		public T GetById(int id)
		{
			return _table.Find(id);
		}

		public IQueryable<T> GetAll()
		{
			return _table;
		}

		public void Remove(T entity)
		{
			_table.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			_table.RemoveRange(entities);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public void Update(T entity)
		{
			_table.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}

		public void Delete(int id)
		{
			T dbEntry = _table.Find(id);
			if (dbEntry != null)
			{
				_table.Remove(dbEntry);
			}
			_context.Entry(new T { Id = id }).State
				= EntityState.Deleted;
		}

		public void Delete(T entity)
		{
			DbEntityEntry dbEntityEntry = _context.Entry(entity);
			if (dbEntityEntry.State != EntityState.Detached)
			{
				dbEntityEntry.State = EntityState.Deleted;
			}
			else
			{
				_table.Attach(entity);
				_table.Remove(entity);
			}
			_context.Entry(entity).State = EntityState.Deleted;
		}

		public int Save()
		{
			return _context.SaveChanges();
		}

		public void SaveContent(Content content)
		{
			if (content.Id == 0)
			{
				_context.Contents.Add(content);
			}
			else
			{
				Content dbEntry = _context.Contents.Find(content.Id);
				if (dbEntry != null)
				{
					dbEntry.Type = content.Type;
					dbEntry.Name = content.Name;
					dbEntry.Description = content.Description;
					dbEntry.MainPhoto = content.MainPhoto;
				}
				
			}
			_context.SaveChanges();
		}

		public Content DeleteContent(int id)
		{
			Content dbEntry = _context.Contents.Find(id);
			if (dbEntry != null)
			{
				_context.Contents.Remove(dbEntry);
				_context.SaveChanges();
			}
			return dbEntry;
		}

		public Task<int> SaveAsync()
		{
			return _context.SaveChangesAsync();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				//if (disposing)
				//	Save();

				_disposedValue = true;
			}
		}
	}
}
