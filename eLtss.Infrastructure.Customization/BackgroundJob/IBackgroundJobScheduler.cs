using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLtss.Infrastructure.Customization.BackgroundJob
{
    public interface IBackgroundJobScheduler
    {
        /// <summary>
        /// Creates a new background job based on a specified type
        /// if delays is specified it schedules it to be enqueued after a given delay.
        /// </summary>
        /// <param name="context">The Execution context/parameters.</param>
        /// <param name="delay">Optional. Delay, after which the job will be enqueued.</param>
        /// <returns>
        /// Unique identifier of the created job.
        /// </returns>
        string Enqueue<T>(object context = null, TimeSpan? delay = null) where T : IBackgroundJob;

        /// <summary>
        /// Creates a new background job based on a specified type
        /// if delays is specified it schedules it to be enqueued after a given delay.
        /// </summary>
        /// <param name="context">The Execution context/parameters.</param>
        /// <param name="delay">Optional. Delay, after which the job will be enqueued.</param>
        /// <returns>
        /// Unique identifier of the created job.
        /// </returns>
        //string Enqueue(Type jobType, IDictionary<string, object> context, TimeSpan? delay);

        /// <summary>
        /// Creates a new background job that will wait for a successful completion
        /// of another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="context">The Execution context/parameters.</param>
        /// <returns>Unique identifier of a created job.</returns>
        string ContinueWith<T>(string parentId, object context = null) where T : IBackgroundJob;

        /// <summary>
        /// Creates a new background job that will wait for a successful completion
        /// of another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="context">The Execution context/parameters.</param>
        /// <returns>Unique identifier of a created job.</returns>
        //string ContinueWith(Type jobType, IDictionary<string, object> context);
    }
}
