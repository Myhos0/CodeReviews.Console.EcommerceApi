using EcommerceAPI.Interfaces;
using EcommerceAPI.Services;

namespace EcommerceAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ISaleService, SaleService>();

        return services;
    }
}