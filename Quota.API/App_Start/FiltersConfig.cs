using Application.Quota.Common.Handler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Quota.API.Filters.ValidateModel;

namespace Quota.API.App_Start
{
    public class FiltersConfig
    {
        protected FiltersConfig() { }

        public static void Register(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(opts => opts.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options => { options.Filters.Add(new HttpResponseExceptionFilter()); });
            services.AddControllers(options => { options.Filters.Add(new ValidateModelAttribute()); });
        }
    }
}
