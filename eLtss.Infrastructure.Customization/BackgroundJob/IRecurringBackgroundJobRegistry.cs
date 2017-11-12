using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLtss.Infrastructure.Customization.BackgroundJob
{
    public interface IRecurringBackgroundJobRegistry
    {
        void AddJob<T>() where T : IRecurringBackgroundJob;

        void AddJob(Type jobType);

        void AddJob(IRecurringBackgroundJob job);


        /// <summary>
        /// Registers previously added jobs with background job service.
        /// IMPORTANT: This method should be called only once after all recurring jobs were added
        /// </summary>
        /// <param name="removeObsoleteJobs">if set to <c>true</c> [remove obsolete jobs].</param>
        void RegisterJobs(bool removeObsoleteJobs);
    }
}
