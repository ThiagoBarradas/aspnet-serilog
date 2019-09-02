using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetSerilog
{
    public static class SerilogService
    {
        public static void SetupSerilog(this IServiceCollection services, SerilogConfiguration configuration)
        {
            services.AddScoped(provider => new LogAdditionalInfo(provider.GetService<IHttpContextAccessor>()));
            services.AddSingleton<ICommunicationLogger>((provider) => new CommunicationLogger(configuration));
        }
    }
}
