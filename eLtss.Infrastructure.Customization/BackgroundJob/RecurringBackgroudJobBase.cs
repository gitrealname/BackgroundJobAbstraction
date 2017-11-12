using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLtss.Infrastructure.Customization.BackgroundJob
{
    public abstract class RecurringBackgroudJobBase : BackgroudJobBase, IRecurringBackgroundJob
    {
        private readonly CronExpression _cronExpression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly string _queueName;

        public RecurringBackgroudJobBase(CronExpression cronExpression, TimeZoneInfo timeZoneInfo = null, string queueName = null)
        {
            _cronExpression = cronExpression;
            _timeZoneInfo = timeZoneInfo;
            _queueName = queueName;
        }

        public RecurringBackgroundJobDescriptor GetJobDescriptor()
        {
            return new RecurringBackgroundJobDescriptor {
                JobId = this.GetType().FullName,
                CronExpression = _cronExpression,
                TimeZoneInfo = _timeZoneInfo,
                QueueName = _queueName
            };
        }
    }
}
