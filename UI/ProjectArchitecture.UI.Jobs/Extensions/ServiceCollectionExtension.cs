using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ProjectArchitecture.UI.Jobs.Infra;

using Quartz.Spi;

namespace ProjectArchitecture.UI.Jobs.Extensions {
    public static class ServiceCollectionExtension {
        public static IServiceCollection AddJobs(this IServiceCollection services) {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationRoot configurationRoot = builder.SetBasePath(AppContext.BaseDirectory)
                                                          .AddJsonFile("appsettings.json")
                                                          .Build();

            return services.AddSingleton(configurationRoot)
                           .AddSingleton<IJobFactory, ServiceProviderAwareJobFactory>()
                           .AddSingleton<IServiceManager, ServiceManager>()
                           .AddTransient<Jobs.FooJob>();
        }
    }
}
