using Hangfire;
using HangfireConsole.Job;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeterKottas.DotNetCore.WindowsService;
using System;

namespace HangfireConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            var services = new ServiceCollection()
             .AddSingleton<MyHangfireJobs>()
             .AddLogging(logging => logging.AddConsole())
             .AddHangfire(configuration =>
             {
                 configuration.UseRedisStorage("localhost");

             });

            ServiceRunner<ExampleService>.Run(config =>
            {
                var name = "WindowsServiceTest";
                config.SetName(name);
                config.SetDescription("WindowsServiceTestDescription");
                config.SetDisplayName("WindowsServiceTestDisplayName");

                config.Service(serviceConfig =>
                {
                    serviceConfig.ServiceFactory((extraArguments, controller) =>
                    {
                        IServiceProvider serviceProvider = services.BuildServiceProvider();
                        return new ExampleService(controller, serviceProvider);
                    });

                    serviceConfig.OnStart((service, extraParams) =>
                    {
                        Console.WriteLine("Service {0} started", name);
                        service.Start();
                    });

                    serviceConfig.OnStop(service =>
                    {
                        Console.WriteLine("Service {0} stopped", name);
                        service.Stop();
                    });

                    serviceConfig.OnError(e =>
                    {
                        Console.WriteLine("Service {0} errored with exception : {1}", name, e.Message);
                    });
                });
            });

        }
    }
}
