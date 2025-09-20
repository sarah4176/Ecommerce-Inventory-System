using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>().ReverseMap();
        }
    }
}
