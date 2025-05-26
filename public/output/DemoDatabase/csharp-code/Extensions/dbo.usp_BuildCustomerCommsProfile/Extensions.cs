using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sql2code.Abstractions.Repositories;
using sql2code.Models;
using sql2code.Repositories;
using sql2code.Services.dbo.usp_BuildCustomerCommsProfile;

namespace sql2code.Extensions.dbo.usp_BuildCustomerCommsProfile
{
    /// <summary>
    /// Extension methods for registering services related to dbo.usp_BuildCustomerCommsProfile
    /// </summary>
    public static class dbo_usp_BuildCustomerCommsProfileExtensions
    {
        /// <summary>
        /// Adds all necessary services, repositories, and mappings for dbo.usp_BuildCustomerCommsProfile
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration instance</param>
        /// <returns>Updated service collection</returns>
        public static IServiceCollection Adddbo_usp_BuildCustomerCommsProfile_Services(this IServiceCollection services, IConfiguration configuration)
        {
            // Register repositories needed for the procedure
            services.AddScoped<IReadRepository<Customer>, Repository<Customer>>();
            services.AddScoped<IReadRepository<Address>, Repository<Address>>();
            services.AddScoped<IReadRepository<LoanApplication>, Repository<LoanApplication>>();
            services.AddScoped<IReadRepository<Loan>, Repository<Loan>>();
            services.AddScoped<IReadRepository<LoanPayment>, Repository<LoanPayment>>();
            services.AddScoped<IReadRepository<CommunicationPreference>, Repository<CommunicationPreference>>();
            services.AddScoped<IReadRepository<MarketingEmailsSent>, Repository<MarketingEmailsSent>>();

            // Register the service
            services.AddScoped<IUspBuildCustomerCommsProfileService, UspBuildCustomerCommsProfileService>();

            return services;
        }
    }
}
