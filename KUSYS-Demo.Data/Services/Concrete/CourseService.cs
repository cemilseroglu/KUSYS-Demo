using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KUSYS_Demo.Data.Repositories.Abstract;
using KUSYS_Demo.Data.Repositories.Concrete;
using KUSYS_Demo.Data.Services.Abstract;
using KUSYS_Demo.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace KUSYS_Demo.Data.Services.Concrete
{
	public class CourseService : EfRepository<Course>, ICourseService
	{
		private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IMapper _mapper;

		public CourseService(
			IDbContextFactory<ApplicationDbContext> contextFactory,
			IHttpContextAccessor httpContextAccessor,
			IDistributedCache distributedCache,
			IMapper mapper
			) :
			base(
				contextFactory,
				httpContextAccessor,
				distributedCache
				)
		{
			_contextFactory = contextFactory;
			_httpContextAccessor = httpContextAccessor;
			_mapper = mapper;
		}
	}
}
