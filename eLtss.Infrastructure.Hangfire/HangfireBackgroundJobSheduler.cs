using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLtss.Infrastructure.Customization.BackgroundJob;
using Hangfire;
using StructureMap;

namespace eLtss.Infrastructure.Hangfire
{
    public class HangfireBackgroundJobSheduler : JobExecutor, IBackgroundJobScheduler
    {

        public HangfireBackgroundJobSheduler(IContainer container) : base(container)
        {
        }

        /// <summary>
        /// Creates a new background job that will wait for a successful completion
        /// of another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="context">The Execution context/parameters.</param>
        /// <returns>Unique identifier of a created job.</returns>
        protected virtual string ContinueWith(string parentId, Type jobType, object context = null)
        {
            var job = CreateJobInstance(jobType);
            var id = BackgroundJob.ContinueWith(parentId, () => ExecuteJob(jobType, context));
            return id;
        }

        /// <summary>
        /// Creates a new background job that will wait for a successful completion
        /// of another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="context">The Execution context/parameters.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public string ContinueWith<T>(string parentId, object context = null) where T : IBackgroundJob
        {
            return ContinueWith(parentId, typeof(T), context);
        }

        /// <summary>
        /// Creates a new background job based on a specified type
        /// if delays is specified it schedules it to be enqueued after a given delay.
        /// </summary>
        /// <param name="context">The Execution context.</param>
        /// <param name="delay">Optional. Delay, after which the job will be enqueued.</param>
        /// <returns>
        /// Unique identifier of the created job.
        /// </returns>
        public string Enqueue(Type jobType, object context = null, TimeSpan? delay = null)
        {
            var job = CreateJobInstance(jobType);
            string id;
            if(delay.HasValue)
            {
                id = BackgroundJob.Schedule(() => ExecuteJob(jobType, context), delay.Value);
            } 
            else
            {
                id = BackgroundJob.Enqueue(() => ExecuteJob(jobType, context));
            }
            
            return id;
        }

        /// <summary>
        /// Creates a new background job based on a specified type
        /// if delays is specified it schedules it to be enqueued after a given delay.
        /// </summary>
        /// <param name="context">The Execution context.</param>
        /// <param name="delay">Optional. Delay, after which the job will be enqueued.</param>
        /// <returns>
        /// Unique identifier of the created job.
        /// </returns>
        public string Enqueue<T>(object context = null, TimeSpan? delay = null) where T : IBackgroundJob
        {
            return Enqueue(typeof(T), context, delay);
        }

        private IBackgroundJob CreateJobInstance(Type jobType)
        {
            var job = _container.GetInstance(jobType);
            var backgroundJob = job as IBackgroundJob;
            if(backgroundJob == null)
            {
                throw new ArgumentException($"{jobType.FullName} must be of type IBackgroundJob.");
            }
            return backgroundJob;
        }
    }
}
