
using DealerAPI.Buisness_Layer.Services;
using DealerApp.Service.Interface;
using DealerApp.Service.Services;
using DealerAPP.Service.Services;

namespace DealerApp.API.Middleware
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderStockAuditService, OrderStockAuditService>();
            services.AddScoped<IAccountDetailsService, AccountDetailsService>();
            services.AddScoped<ICustomerSupportService, CustomerSupportService>();
            services.AddScoped<IPVAMakeService, PVAMakeService>();
            services.AddScoped<ICustomerSupportService, CustomerSupportService>();
            services.AddScoped<IPVAModelService, PVAModelService>();
            services.AddScoped<IPVAVariantService, PVAVariantService>();
            services.AddScoped<IPVAYearOfRegService, PVAYearOfRegService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IPVOpenMarketService, PVOpenMarketService>();
            services.AddScoped<IPVNewCarDealerService, PVNewCarDealerService>();
            services.AddScoped<IPVAggregatorService, PVAggregatorService>();
            services.AddScoped<IPaymentProofImgService, PaymentProofImgService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ILoginUserPhoneService, LoginUserPhoneService>();
            services.AddScoped<IProucurementService, ProucurementService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IProfileInformationService, ProfileInformationService>();
            services.AddScoped<IStockAuditService, StockAuditService>();

            // Add other services here...

            return services;
        }
    }
}