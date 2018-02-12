using System;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using HangfireApp.Jobs;

namespace HangfireApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var jobId = BackgroundJob.Enqueue<MyHangfireJobs>(jobs => jobs.Run("Enqueue"));

            var jobIdT = BackgroundJob.Schedule<MyHangfireJobs>(jobs => jobs.Run("Schedule"), DateTimeOffset.UtcNow.AddSeconds(10));

            var jobIdTT = "RecurringSendGetRequest";
            RecurringJob.AddOrUpdate<MyHangfireJobs>(jobIdTT, jobs => jobs.Run("AddOrUpdate"), Cron.Minutely());

            BackgroundJob.ContinueWith<MyHangfireJobs>(jobId, jobs => jobs.Run("Enqueue--ContinueWith"), JobContinuationOptions.OnlyOnSucceededState);
            BackgroundJob.ContinueWith<MyHangfireJobs>(jobIdT, jobs => jobs.Run("Schedule--ContinueWith"), JobContinuationOptions.OnlyOnSucceededState);

            //BackgroundJob.ContinueWith<MyHangfireJobs>(jobIdTT, jobs => jobs.Run("AddOrUpdate--ContinueWith"), JobContinuationOptions.OnlyOnSucceededState);
            return Ok("Ok");
        }


    }
}
