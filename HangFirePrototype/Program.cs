using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLtss.Infrastructure.Customization.BackgroundJob;
using eLtss.Infrastructure.Customization.JobProcessor;
using eLtss.Infrastructure.Hangfire;
using Hangfire;
using StructureMap;

namespace HangFirePrototype
{
    public interface IPrinter
    {
        void Print(string message);
    }

    public class Printer : IPrinter
    {

        public void Print(string message)
        {
            Console.WriteLine(message);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var connString = "Host=localhost;Port=5432;Database=hangfire;Username=postgres;Password=123456;Pooling=true;MaxPoolSize=1024;";
            var container = new Container();
            container.Configure(_ =>
            {
                _.For<IPrinter>().Use<Printer>();
                _.For<IBackgroundJobServerManager>().Singleton().Use(new HangfireBackgroundJobServerManager(connString));
                _.For<IRecurringBackgroundJobRegistry>().Singleton().Use(new HangfireRecurringBackgroundJobRegistry(container));
                _.For<IBackgroundJobScheduler>().Singleton().Use(new HangfireBackgroundJobSheduler(container));

                _.Scan(scaner => {
                    scaner.AssembliesAndExecutablesFromApplicationBaseDirectory(a =>
                    {
                        Console.WriteLine(a.GetName());
                        return true;
                    });
                    //scaner.AddAllTypesOf<IRecurringBackgroundJob>();
                    scaner.Convention<StructureMapBackgroundJobTypeScannerConvention>();
                });

            });

            var registy = container.GetInstance<IRecurringBackgroundJobRegistry>();

            var scheduler = container.GetInstance<IBackgroundJobScheduler>();

            using(var server = container.GetInstance<IBackgroundJobServerManager>())
            {
                server.Start();

                //RecurringJob.AddOrUpdate("Test", () => Console.WriteLine($"Test Minutely: {DateTime.Now}."), Cron.MinuteInterval(1));
                //RecurringJob.AddOrUpdate("Test1", () => Console.WriteLine($"Test Minutely: {DateTime.Now}."), Cron.MinuteInterval(1));
                //RecurringJob.AddOrUpdate("Test2", () => Console.WriteLine($"Test Minutely: {DateTime.Now}."), Cron.MinuteInterval(1));

                var registry = container.GetInstance<IRecurringBackgroundJobRegistry>();
                foreach(var t in StructureMapBackgroundJobTypeScannerConvention.RecurringJobTypes)
                {
                    registry.AddJob(t);
                }
                StructureMapBackgroundJobTypeScannerConvention.RecurringJobTypes.Clear();
                registry.RegisterJobs(true);

                Console.WriteLine();
                Console.WriteLine();
                while(true)
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Enter 1: for manual job; 2: for continue with; 3: to trigger recurring. 4: enqueue with delay");
                    Console.ForegroundColor = color;
                    var cmd = Console.ReadLine();

                    switch(cmd)
                    {
                        case "1":
                            scheduler.Enqueue<Job1>(new { a = 10 });
                            break;
                        case "2":
                            var id = scheduler.Enqueue<Job1>();
                            scheduler.ContinueWith<Job2>(id, new { a = "data", b = 10, c = "c" });
                            break;
                        case "3":
                            var id2 = scheduler.Enqueue<Job1>();
                            scheduler.ContinueWith<JobRecurringJob>(id2, new { a = "data", b = 10 });
                            break;
                        case "4":
                            scheduler.Enqueue<Job1>(null, TimeSpan.FromSeconds(3));
                            break;
                        case "0":
                            return;
                            break;
                    }   
                }
            }

        }
    }
}
