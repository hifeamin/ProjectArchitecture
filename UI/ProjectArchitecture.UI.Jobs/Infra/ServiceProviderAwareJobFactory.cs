using System;
using System.Collections.Concurrent;

using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Spi;

namespace ProjectArchitecture.UI.Jobs.Infra {
    public class ServiceProviderAwareJobFactory : IJobFactory {
        private readonly IServiceProvider _serviceProvider;
        private readonly bool _logStatus;
        private readonly ConcurrentDictionary<IJob, IServiceScope> _scopePool;

        public ServiceProviderAwareJobFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
            _scopePool = new ConcurrentDictionary<IJob, IServiceScope>();
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) {
            IServiceScope scope = null;
            try {
                scope = _serviceProvider.CreateScope();
                IJob job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
                if (job == null) {
                    scope.Dispose();
                    scope = null;
                    return default;
                }
                if (!_scopePool.TryAdd(job, scope)) {
                    scope.Dispose();
                    scope = null;
                    return default;
                }
                return job;
            }
            catch (Exception e) {
                //Log the exception
                scope?.Dispose();
                return default;
            }
        }

        public void ReturnJob(IJob job) {
            if (_scopePool.TryRemove(job, out IServiceScope scope)) {
                scope.Dispose();
            }

            IDisposable disposableJob = job as IDisposable;
            if (disposableJob != null) {
                disposableJob.Dispose();
            }

        }
    }
}
