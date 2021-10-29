using Microsoft.Extensions.DependencyInjection;

namespace ProjectArchitecture.Bootstrapper.Extensions {
    public static class ServiceCollectionExtension {
        public static IServiceCollection AddServices(this IServiceCollection @this,
                                                         IServiceCollectionConfigurationManager serviceCollectionConfigurationManager = null) {
            serviceCollectionConfigurationManager ??= new DefaultServiceCollectionConfigurationManager();
            serviceCollectionConfigurationManager.RegisterUtils(@this)
                                                 .RegisterRepositories(@this)
                                                 .RegisterServices(@this);
            return @this;
        }
    }
}
