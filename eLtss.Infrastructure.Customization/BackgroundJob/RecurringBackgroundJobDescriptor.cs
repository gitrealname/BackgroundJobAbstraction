using System;

namespace eLtss.Infrastructure.Customization.BackgroundJob
{
    public class RecurringBackgroundJobDescriptor
    {
        public string JobId { get; set; }

        public CronExpression CronExpression { get; set; } 

        public TimeZoneInfo TimeZoneInfo { get; set; }

        public string QueueName { get; set; }
    }
}