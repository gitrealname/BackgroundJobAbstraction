using System;
using System.Collections.Generic;

namespace eLtss.Infrastructure.Customization.BackgroundJob
{

    public interface IBackgroundJob : IDisposable
    {
        void Execute(BackgroundJobContextDictionary context);
    }
}
