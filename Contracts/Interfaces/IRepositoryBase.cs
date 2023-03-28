using System.Linq;
using System.Linq.Expressions;

namespace Contracts.Interfaces;
//These interface holds methods for RepositoryBase class
public interface IRepositoryBase<T>
{
	IQueryable<T> FindAll(bool trackChanges);
	IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

	void Create(T entity);
	void Update(T entity);
	void Delete(T entity);
}