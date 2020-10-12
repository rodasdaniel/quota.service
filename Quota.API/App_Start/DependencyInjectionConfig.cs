using Application.Quota.Business.Quota;
using Infrastructure.Quota.Agents.Client;
using Infrastructure.Quota.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Quota.API.App_Start
{
    public class DependencyInjectionConfig
    {
        protected DependencyInjectionConfig() { }
        public static void Register(IServiceCollection services)
        {
            //Business
            services.AddScoped(typeof(IQuotaBusiness), typeof(QuotaBusiness));

            //Repository
            services.AddScoped(typeof(IQuotaRepository), typeof(QuotaRepository));

            //Agents
            services.AddScoped(typeof(IClientProvider), typeof(ClientProvider));
        }
    }
}
