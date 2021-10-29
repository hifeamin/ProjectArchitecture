using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ProjectArchitecture.Business.Interfaces;
using ProjectArchitecture.Business.Services;
using ProjectArchitecture.Data.Impl.Sql;
using ProjectArchitecture.Data.Repositories;
using ProjectArchitecture.Domain.Service;

namespace ProjectArchitecture.Bootstrapper {
    public class DefaultServiceCollectionConfigurationManager : IServiceCollectionConfigurationManager {
        public IServiceCollectionConfigurationManager RegisterRepositories(IServiceCollection service) {
            service.AddTransient<IDataRepository, DataRepository>();
            return this;
        }

        public IServiceCollectionConfigurationManager RegisterServices(IServiceCollection service) {
            service.AddTransient<IService, Service>()
                   .AddTransient<IValidationService, ValidationService>();
            return this;
        }

        public IServiceCollectionConfigurationManager RegisterUtils(IServiceCollection service) {
            MapperConfiguration config = new MapperConfiguration(ConfigDataModels);
            IMapper mapper = config.CreateMapper();
            service.AddSingleton<IMapper>(mapper);
            return this;
        }

        protected virtual void ConfigDataModels(IMapperConfigurationExpression config) {
        }
    }
}
