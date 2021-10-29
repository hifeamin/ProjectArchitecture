using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using ProjectArchitecture.Bootstrapper.Extensions;
using ProjectArchitecture.UI.Jobs.Extensions;

namespace ProjectArchitecture.UI.Jobs {
    class Program {
        protected static IServiceProvider ServiceProvider { get; private set; }
        private static readonly ManualResetEvent resetEvent = new(false);

        static void Main(string[] args) {
            ConfigDi();

            Console.WriteLine("Started.");
            Console.CancelKeyPress += (sender, e) => {
                Console.WriteLine("Shutting Down ...");
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
    }
}
