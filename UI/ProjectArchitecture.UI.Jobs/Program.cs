using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using ProjectArchitecture.Bootstrapper.Extensions;
using ProjectArchitecture.UI.Jobs.Extensions;
using ProjectArchitecture.UI.Jobs.Infra;
using ProjectArchitecture.UI.Jobs.Infra.Models;

namespace ProjectArchitecture.UI.Jobs {
    class Program {
        protected static IServiceProvider ServiceProvider { get; private set; }
        private static readonly ManualResetEvent resetEvent = new(false);

        static void Main(string[] args) {
            ConfigDi();

            IServiceManager serviceManager = ServiceProvider.GetService<IServiceManager>();
            serviceManager.JobConfigs = GetJobConfiguration();
            serviceManager.InitializeJobs();
            serviceManager.Start();

            Console.WriteLine("Started.");
            Console.CancelKeyPress += (sender, e) => {
                Console.WriteLine("Shutting Down ...");
                serviceManager.ShutDown();
                resetEvent.Set();
            };
            resetEvent.WaitOne();
        }

        private static void ConfigDi() {
            IServiceCollection services = new ServiceCollection();
            services.AddServices()
                    .AddJobs();
            ServiceProvider = services.BuildServiceProvider();
        }

        private static IEnumerable<JobConfigurationModel> GetJobConfiguration() {
            JobConfigurationModel jobConfig = new JobConfigurationModel() {
                Enable = true,
                Interval = new TimeSpan(0, 0, 5),
                UseCron = false,
                JobType = typeof(Jobs.FooJob)
            };
            return new List<JobConfigurationModel>(1) { jobConfig };
        }
    }
}
