using System.Collections.Generic;
using System.Threading;

using ProjectArchitecture.UI.Jobs.Infra.Models;

using Quartz;

namespace ProjectArchitecture.UI.Jobs.Infra {
    public interface IServiceManager {
        IEnumerable<JobConfigurationModel> JobConfigs { get; set; }
        IScheduler Scheduler { get; }
        void InitializeJobs();
        void AddJob(JobConfigurationModel config);
        void Start();
        void ShutDown();
        void Pause(string jobKey = null);
        void Resume(string jobKey = null);
        void Interrupt(string jobKey, CancellationToken cancellationToken = default);
    }
}
