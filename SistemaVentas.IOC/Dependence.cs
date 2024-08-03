using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVentas.BusinessLogic.Implementation;
using SistemaVentas.BusinessLogic.Interface;
using SistemaVentas.Dal;
using SistemaVentas.Dal.Implementation;
using SistemaVentas.Dal.Interface;
using SistemaVentas.Entities;

namespace SistemaVentas.IOC
{
    public static class Dependence
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataBase_Application_Sales_MVCContext>(options =>
            {
                options.UseSqlServer(configuration.GetRequiredSection("ConfigurationApplication:ConnectionString_ApplicationSales").Value);
            });

            //INJECTAR LOS SERVICIOS DE REPOSITORY
            services.AddTransient(typeof(IGenericPrincipalRepository<>), typeof(GenericPrincipalRepository<>));
            services.AddScoped<ISalesRepository,SalesRepository>();
            services.AddScoped<IServicesMailApplication, ServicesMailApplication>();
            services.AddScoped<IServicesFirebaseStorage,ServiceFirebaseStorageApplication>();
            services.AddScoped<IServicesProfits, ServicesProfitsApplication>();
            services.AddScoped<IServicesRolesApplication,ServicesRolesApplication>();
            services.AddScoped<IServicesUsers, ServicesUsersApplication>();
            services.AddScoped<IServicesBusiness, ServicesBusinessApplication>();
            services.AddScoped<IServicesCategoryApplication,  ServicesCategoryApplication>();
            services.AddScoped<IServicesProductApplication, ServicesProductApplication>();
            services.AddScoped<IServicesSaleType,ServicesSaleTypeApplication>();
            services.AddScoped<IServicesSales, ServicesSalesApplication>();
            services.AddScoped<IServicesDashBoard>(provider =>
            {
                var salesRepository = provider.GetRequiredService<ISalesRepository>();
                var saleDetailRepository = provider.GetRequiredService<IGenericPrincipalRepository<SaleDetail>>();
                var categoryRepository = provider.GetRequiredService<IGenericPrincipalRepository<Category>>();
                var productRepository = provider.GetRequiredService<IGenericPrincipalRepository<Product>>();

                // Proporciona el valor de startDate
                DateTime startDate = DateTime.Now; // O cualquier lógica que necesites para obtener el DateTime

                return new ServicesDashBoardApplication(salesRepository, saleDetailRepository, categoryRepository, productRepository, startDate);
            });




        }
    }
}
