using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ProjectArchitecture.UI.Jobs.Infra.Models;

using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace ProjectArchitecture.UI.Jobs.Infra {
    public class ServiceManager : IServiceManager {
        private readonly IJobFactory _jobFactory = null;

        public ServiceManager(IJobFactory jobFactory,
                              IEnumerable<JobConfigurationModel> jobConfigurations) {
            _jobFactory = jobFactory;
            Scheduler = Task.Run(async () => await StdSchedulerFactory.GetDefaultScheduler()).Result;
            Scheduler.JobFactory = _jobFactory;

            JobConfigs = jobConfigurations;
        }

        public IScheduler Scheduler { get; private set; }
        public IEnumerable<JobConfigurationModel> JobConfigs { get; set; }

        public void Pause(string jobKey = null) {
            if (string.IsNullOrEmpty(jobKey))
                Scheduler.PauseAll();
            else
                Scheduler.PauseJob(new JobKey(jobKey));
        }

        public void Resume(string jobKey = null) {
            if (string.IsNullOrEmpty(jobKey))
                Scheduler.ResumeAll();
            else
                Scheduler.ResumeJob(new JobKey(jobKey));
        }

        public void Interrupt(string jobKey, CancellationToken cancellationToken = default) {
            Scheduler.Interrupt(new JobKey(jobKey), cancellationToken);
        }

        public void ShutDown() {
            Scheduler.Shutdown();
        }

        public void Start() {
            if (Scheduler.IsShutdown) {
                InitializeJobs();
            }

            Scheduler.Start();
        }

        public void InitializeJobs() {
            if (JobConfigs == null) {
                throw new NullReferenceException("JobConfigs Cann't be null");
            }

            foreach (var config in JobConfigs.Where(j => j.Enable)) {
                AddJob(config);
            }
        }

        public void AddJob(JobConfigurationModel config) {
            if (string.IsNullOrEmpty(config.Id)) {
                config.Id = config.JobType.Name;
            }

            JobBuilder jobBuilder = JobBuilder.Create(config.JobType);
            jobBuilder.WithIdentity(config.Id);
            if (config.JobData != null && config.JobData.Any()) {
                jobBuilder.SetJobData(new JobDataMap(config.JobData));
            }

            IJobDetail job = jobBuilder.Build();

            TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                                                          .WithIdentity(config.Id)
                                                          .StartAt(DateTimeOffset.Now.Add(config.Delay));
            if (!config.UseCron) {
                if (!config.Interval.HasValue)
                    throw new Exception("Please set interval.");

                triggerBuilder.WithSimpleSchedule(x => x.WithInterval(config.Interval.Value)
                                                        .WithMisfireHandlingInstructionNextWithRemainingCount()
                                                        .RepeatForever());
            }
            else {
                if (string.IsNullOrEmpty(config.CronSchedule))
                    throw new Exception("Please set cronschedule.");

                triggerBuilder.WithCronSchedule(config.CronSchedule);
            }

            ITrigger trigger = triggerBuilder.Build();
            Scheduler.ScheduleJob(job, trigger);
        }
    }
}
