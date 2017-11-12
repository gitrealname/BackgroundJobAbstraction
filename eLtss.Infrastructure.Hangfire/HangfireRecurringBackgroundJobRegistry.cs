using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using CronExpressionDescriptor;
using eLtss.Infrastructure.Customization.BackgroundJob;
using Hangfire;
using Hangfire.Storage;
using StructureMap;

namespace eLtss.Infrastructure.Hangfire
{
    public class HangfireRecurringBackgroundJobRegistry : JobExecutor, IRecurringBackgroundJobRegistry
    {
        private static Dictionary<Type, RecurringBackgroundJobDescriptor> _registeredJobs = new Dictionary<Type, RecurringBackgroundJobDescriptor>();
        private static bool _registered = false;
        public HangfireRecurringBackgroundJobRegistry(IContainer container) : base(container)
        {
        }

        public void AddJob(Type jobType)
        {
            var job = _container.GetInstance(jobType);
            var recurringJob = job as IRecurringBackgroundJob;
            try
            {
                if(recurringJob == null)
                {
                    throw new ArgumentException($"{jobType.FullName} must be of type IRecurringBackgroundJob.");
                }
                AddJob(recurringJob);
            }
            finally
            {
                if(recurringJob != null)
                {
                    recurringJob.Dispose();
                }
            }
        }

        public void AddJob<T>() where T : IRecurringBackgroundJob
        {
            AddJob(typeof(T));
        }

        public void AddJob(IRecurringBackgroundJob recurringJob)
        {
            var descriptor = recurringJob.GetJobDescriptor();
            _registeredJobs.Add(recurringJob.GetType(), descriptor);
        }

        /// <summary>
        /// Registers previously added jobs with background job service.
        /// </summary>
        /// <param name="removeObsoleteJobs">if set to <c>true</c> [remove obsolete jobs].</param>
        public void RegisterJobs(bool removeObsoleteJobs)
        {
            if(_registered)
            {
                throw new InvalidOperationException("Recurring Jobs already registered");
            }
            _registered = true;

            var ids = new HashSet<string>();
            foreach(var kv in _registeredJobs)
            {
                var type = kv.Key;
                var desc = kv.Value;
                ids.Add(desc.JobId);
                RecurringJob.AddOrUpdate(desc.JobId, () => ExecuteJob(type, null), desc.CronExpression.Value, desc.TimeZoneInfo, desc.QueueName == null ? "default" : desc.QueueName);

                var cronMsg = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(desc.CronExpression.Value, new Options {CasingType = CasingTypeEnum.Sentence, Use24HourTimeFormat = true});
                var msg = $"Registered Recurring Job Id: {desc.JobId}; Runs: {cronMsg}";
                System.Diagnostics.Trace.WriteLine(msg);
            }

            if(removeObsoleteJobs)
            {
                using(var connection = JobStorage.Current.GetConnection())
                {
                    var recurring = connection.GetRecurringJobs();

                    foreach(var recurringJob in connection.GetRecurringJobs())
                    {
                        if(!ids.Contains(recurringJob.Id))
                        {
                            RecurringJob.RemoveIfExists(recurringJob.Id);
                            var msg = $"Removed Obsolete Recurring Job Id: {recurringJob.Id}";
                            System.Diagnostics.Trace.WriteLine(msg);
                        }
                    }
                }
            }

            System.Diagnostics.Trace.Flush();

            _registeredJobs.Clear();
        }
    }
}
