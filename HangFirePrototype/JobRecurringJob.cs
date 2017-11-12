using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLtss.Infrastructure.Customization.BackgroundJob;

namespace HangFirePrototype
{
    public class JobRecurringJob : RecurringBackgroudJobBase
    {
        private readonly IPrinter _printer;

        public JobRecurringJob(IPrinter printer) : base(CronExpressionBuilder.MinuteInterval(1))
        {
            _printer = printer;
        }
        public override void Execute(BackgroundJobContextDictionary context)
        {
            _printer.Print($"JobRecurringJob: context keys count: {context.Keys.Count}");
        }
    }
}
