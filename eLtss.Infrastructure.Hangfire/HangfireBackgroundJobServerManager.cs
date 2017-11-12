using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLtss.Infrastructure.Customization.BackgroundJob;
using eLtss.Infrastructure.Customization.JobProcessor;
using Hangfire;
using Hangfire.PostgreSql;
using StructureMap;

namespace eLtss.Infrastructure.Hangfire
{
    public class HangfireBackgroundJobServerManager : IBackgroundJobServerManager
    {
        private readonly string _postgresStorageConnectionString;
        private bool IsConfigured = false;
        private BackgroundJobServer Server;
        private bool Disposed = false;

        public HangfireBackgroundJobServerManager(string postgresStorageConnectionString)
        {
            _postgresStorageConnectionString = postgresStorageConnectionString;
        }

        public void Start()
        {
            Configure();
            //JobStorage.Current = new PostgreSqlStorage(_postgresStorageConnectionString);
            Server = new BackgroundJobServer();
        }

        public void Stop()
        {
            Server.SendStop();
            Server.Dispose();
        }

        protected virtual void Configure()
        {
            if(IsConfigured)
            {
                return;
            }
            IsConfigured = true;
            GlobalConfiguration.Configuration
                //.UseLogProvider()
                //.UseFilter()
                //.UseActivator(new xyzActivator()))
                .UsePostgreSqlStorage(_postgresStorageConnectionString);
        }


        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected void Dispose(bool disposing)
        {
            if(Disposed)
                return;

            if(disposing)
            {
                // Free any other managed objects here.
                Server.Dispose();
            }

            // Free any unmanaged objects here.
            Disposed = true;
        }

        ~HangfireBackgroundJobServerManager()
        {
            Dispose(false);
        }
    }
}
