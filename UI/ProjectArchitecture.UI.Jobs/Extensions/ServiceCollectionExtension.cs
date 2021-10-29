using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectArchitecture.UI.Jobs.Extensions {
    public static class ServiceCollectionExtension {
        public static IServiceCollection AddJobs(this IServiceCollection services) {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationRoot configurationRoot = builder.SetBasePath(AppContext.BaseDirectory)
                                                          .AddJsonFile("appsettings.json")
                                                          .Build();

            return services.AddSingleton(configurationRoot);
        }
    }
}
