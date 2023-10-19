using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Humanizer;
using KUSYS_Demo.Entities.Concrete;
using KUSYS_Demo.Models;

namespace KUSYS_Demo.Mapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			//CreateMap<Dto, Class>().ReverseMap();
			CreateMap<CourseCreateViewModel, Course>().ReverseMap();
		}

	}
}
