using System;
using System.Collections.Generic;

namespace ProjectArchitecture.UI.Jobs.Infra.Models {
    public class JobConfigurationModel {
        public string Id { get; set; }
        public Type JobType { get; set; }
        public TimeSpan? Interval { get; set; }
        public string CronSchedule { get; set; }
        public bool UseCron { get; set; }
        public TimeSpan Delay { get; set; } = TimeSpan.Zero;
        public IDictionary<string, object> JobData { get; set; } = new Dictionary<string, object>();
        public bool Enable { get; set; } = true;
    }
}
