using System;

namespace eLtss.Infrastructure.Customization.BackgroundJob
{
    public class CronExpression
    {
        private readonly string _cronExpression;

        public CronExpression(string cronExpression)
        {
            if(string.IsNullOrWhiteSpace(cronExpression)) throw new ArgumentNullException(nameof(cronExpression));
            var lst = cronExpression.Split(' ');
            if(lst.Length != 5) throw new ArgumentException("", nameof(cronExpression));

            _cronExpression = cronExpression;
        }

        public string Value => _cronExpression;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
