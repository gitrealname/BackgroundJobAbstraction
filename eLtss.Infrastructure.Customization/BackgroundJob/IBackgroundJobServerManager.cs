using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLtss.Infrastructure.Customization.JobProcessor
{
    public interface IBackgroundJobServerManager : IDisposable
    {
        void Start();

        void Stop();
    }
}
