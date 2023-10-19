using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KUSYS_Demo.Data.Repositories.Abstract;
using KUSYS_Demo.Entities.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace KUSYS_Demo.Data.Repositories.Concrete
{
	public abstract class EfRepository<T> : IRepository<T> where T : BaseEntity, new()
	{
		private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IDistributedCache _distributedCache;

		public EfRepository(
			IDbContextFactory<ApplicationDbContext> contextFactory,
			IHttpContextAccessor httpContextAccessor,
			IDistributedCache distributedCache
			)
		{
			_contextFactory = contextFactory;
			_httpContextAccessor = httpContextAccessor;
			_distributedCache = distributedCache;
		}


		/// <summary>
		/// Get by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		public virtual async Task<T> GetById(string id)
		{
			using (var _context = _contextFactory.CreateDbContext())
			{
				
				try
				{
					var data = _context.Set<T>().Find(id);

					if (data == null)
					{
						
					}

					return data;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		/// <summary>
		/// Adds new data to database
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public virtual async Task<T> Add(T entity)
		{
			using (var _context = _contextFactory.CreateDbContext())
			{

				try
				{
					_context.Set<T>().Add(entity);
					_context.SaveChanges();

					return entity;

				}
				catch (DbUpdateException ex)
				{
					throw ex;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		/// <summary>
		/// Adds new data to database
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public virtual async Task<IEnumerable<T>> AddRange(IEnumerable<T> entity)
		{
			using (var _context = _contextFactory.CreateDbContext())
			{
				try
				{
					_context.Set<T>().AddRange(entity);
					_context.SaveChanges();

					return entity;

				}
				catch (DbUpdateException ex)
				{
					throw ex;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		/// <summary>
		/// Get all data 
		/// This method has cache
		/// </summary>
		/// <returns></returns>
		public virtual async Task<IEnumerable<T>> GetAll()
		{
			using (var _context = _contextFactory.CreateDbContext())
			{
				try
				{
					var data = _context.Set<T>().ToList();
					return data.OrderByDescending(x => x.Created);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		/// <summary>
		/// Remove by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public virtual async Task<bool> Remove(string id)
		{
			using (var _context = _contextFactory.CreateDbContext())
			{
				try
				{
					var data = _context.Set<T>().FirstOrDefault(p => String.Equals(p.Id, id));
					if (data == null)
					{ };

					_context.Set<T>().Remove(data);
					_context.SaveChanges();
					return true;

				}
				catch (DbUpdateException ex)
				{
					throw ex;
				}
				catch (Exception ex)
				{
					throw ex;
				}

			}

		}

		/// <summary>
		/// Update data
		/// </summary>
		/// <param name="id"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		public virtual async Task<T> Update(string id, T entity)
		{

			using (var _context = _contextFactory.CreateDbContext())
			{
				try
				{
					var data = _context.Set<T>().SingleOrDefault(x => String.Equals(x.Id, id));

					if (data == null)
						{
						return null;
						};

					_context.ChangeTracker.Clear();
					_context.Set<T>().Update(entity);
					_context.SaveChanges();
					return data;
				}
				catch (DbUpdateException ex)
				{
					throw ex;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}
	}
}

