using System.Collections.Generic;

namespace eLtss.Infrastructure.Customization.BackgroundJob
{
    public interface IRecurringBackgroundJob : IBackgroundJob
    {
        RecurringBackgroundJobDescriptor GetJobDescriptor();
    }
}
