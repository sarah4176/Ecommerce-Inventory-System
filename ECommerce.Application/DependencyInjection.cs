using ECommerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
