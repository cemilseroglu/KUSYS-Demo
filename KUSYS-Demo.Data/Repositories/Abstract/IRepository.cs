using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KUSYS_Demo.Entities.Abstract;

namespace KUSYS_Demo.Data.Repositories.Abstract
{
	public interface IRepository<TEntity> where TEntity : BaseEntity
	{
		Task<TEntity> Add(TEntity entity);
		Task<TEntity> GetById(string id);
		Task<IEnumerable<TEntity>> GetAll();
		Task<TEntity> Update(string id, TEntity entity);
		Task<bool> Remove(string id);
		Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entity);
	}
}
