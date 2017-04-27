using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LvivAdviser.Domain.Abstract.Interfaces
{
	public interface IRepository<TEntity> : IDisposable 
		where TEntity : class 
	{
		TEntity GetById(int id);
		IQueryable<TEntity> GetAll();
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

		void Add(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);

		void Remove(TEntity entity);
		void RemoveRange(IEnumerable<TEntity> entities);
		void Delete(int id);
		void Delete(TEntity entity);

		void Update(TEntity entity);
		void SaveContent(Content content);
                Content DeleteContent(int contentId);	

		int Save();
		Task<int> SaveAsync();
	}
}
