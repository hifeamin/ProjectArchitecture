using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quartz;

using ProjectArchitecture.Domain.Service;

namespace ProjectArchitecture.UI.Jobs.Jobs {
    public class FooJob : IJob {
        private readonly IService _service;

        public FooJob(IService service) {
            _service = service;
        }

        public Task Execute(IJobExecutionContext context) {
            Console.WriteLine(DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
