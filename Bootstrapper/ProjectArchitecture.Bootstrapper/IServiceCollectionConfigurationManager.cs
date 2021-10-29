using Microsoft.Extensions.DependencyInjection;

namespace ProjectArchitecture.Bootstrapper {
    public interface IServiceCollectionConfigurationManager {
        IServiceCollectionConfigurationManager RegisterRepositories(IServiceCollection service);
        IServiceCollectionConfigurationManager RegisterServices(IServiceCollection service);
        IServiceCollectionConfigurationManager RegisterUtils(IServiceCollection service);
    }
}
