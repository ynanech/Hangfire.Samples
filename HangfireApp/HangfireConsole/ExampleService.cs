using Hangfire;
using Hangfire.Server;
using HangfireConsole.Job;
using Microsoft.Extensions.DependencyInjection;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;

namespace HangfireConsole
{
    public class ExampleService : IMicroService
    {
        private IMicroServiceController controller;
        private BackgroundJobServer _backgroundJobServer;
        private IServiceProvider _serviceProvider;
        public ExampleService()
        {
            controller = null;
        }

        public ExampleService(IMicroServiceController controller, IServiceProvider serviceProvider)
        {
            this.controller = controller;
            _serviceProvider = serviceProvider;
        }
        public void Start()
        {
            Console.WriteLine("I started");

            var storage = _serviceProvider.GetRequiredService<JobStorage>();
            var options = _serviceProvider.GetService<BackgroundJobServerOptions>() ?? new BackgroundJobServerOptions();
            var additionalProcesses = _serviceProvider.GetServices<IBackgroundProcess>();

            _backgroundJobServer = new BackgroundJobServer(options, storage, additionalProcesses);


            BackgroundJob.Enqueue<MyHangfireJobs>(jobs => jobs.Run("Enqueue1"));
            BackgroundJob.Enqueue<MyHangfireJobs>(jobs => jobs.Run("Enqueue2"));

            BackgroundJob.Schedule<MyHangfireJobs>(jobs => jobs.Run("Schedule"), DateTimeOffset.UtcNow.AddSeconds(15));
            var jobIdTT = "RecurringSendGetRequest";
            RecurringJob.AddOrUpdate<MyHangfireJobs>(jobIdTT, jobs => jobs.Run("AddOrUpdate"), Cron.Minutely());
        }

        public void Stop()
        {
            Console.WriteLine("I stopped");

            _backgroundJobServer.SendStop();
            _backgroundJobServer.Dispose();

        }
    }
}
