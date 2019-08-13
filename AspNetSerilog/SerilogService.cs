using Microsoft.Extensions.DependencyInjection;

namespace AspNetSerilog
{
    public static class SerilogService
    {
        public static void SetupSerilog(this IServiceCollection services, SerilogConfiguration configuration)
        {
            services.AddSingleton<ICommunicationLogger>((provider) => new CommunicationLogger(configuration));
        }
    }
}
