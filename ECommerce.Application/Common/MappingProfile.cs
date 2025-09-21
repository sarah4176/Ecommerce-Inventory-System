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
            CreateMap<Product, ProductDTO>();
            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<CreateProductDTO, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Category, CategoryBasicDTO>();
        }
    }
    }

