using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLtss.Infrastructure.Customization.BackgroundJob;
using StructureMap;

namespace eLtss.Infrastructure.Hangfire
{
    public class JobExecutor
    {
        protected static IContainer _container;

        public JobExecutor(IContainer container)
        {
            _container = container;
        }
        public static void ExecuteJob(Type jobType, object context)
        {
            var ctxDict = context == null ? new BackgroundJobContextDictionary() : new BackgroundJobContextDictionary(context);
            using(var childContainer = _container.CreateChildContainer())
            {
                using(var job = childContainer.GetInstance(jobType) as IBackgroundJob)
                {
                    job.Execute(ctxDict);
                }
            }
        }
    }
}
