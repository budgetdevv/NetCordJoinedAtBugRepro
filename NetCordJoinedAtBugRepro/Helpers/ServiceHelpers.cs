using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NetCordJoinedAtBugRepro.Helpers
{
    public static class ServiceHelpers
    {
        public static IServiceCollection AddHostedSingletonService<ServiceT>(this IServiceCollection services)
            where ServiceT : class, IHostedService
        {
            return services
                .AddSingleton<ServiceT>()
                .AddHostedService<ServiceT>(x => x.GetRequiredService<ServiceT>());
        }
    }
}
