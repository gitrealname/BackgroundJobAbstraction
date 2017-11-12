using System;
using System.Collections.Generic;

namespace eLtss.Infrastructure.Customization.BackgroundJob
{
    public abstract class BackgroudJobBase : IBackgroundJob
    {
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected void Dispose(bool disposing)
        {
            if(disposed)
                return;

            if(disposing)
            {
                // Free any other managed objects here.
                DisposeManagedObjects();
            }

            // Free any unmanaged objects here.
            DisposeUnmanagedObjects();
            disposed = true;
        }

        ~BackgroudJobBase()
        {
            Dispose(false);
        }

        public virtual void DisposeManagedObjects() { }

        public virtual void DisposeUnmanagedObjects() { }

        public abstract void Execute(BackgroundJobContextDictionary context);

    }
}
