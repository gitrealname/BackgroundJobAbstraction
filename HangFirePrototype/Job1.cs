using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLtss.Infrastructure.Customization.BackgroundJob;

namespace HangFirePrototype
{
    public class Job1 : BackgroudJobBase
    {
        private readonly IPrinter _printer;

        public Job1(IPrinter printer)
        {
            _printer = printer;
        }
        public override void Execute(BackgroundJobContextDictionary context)
        {
            _printer.Print($"job1: context keys count: {context.Keys.Count}");
        }
    }
}
