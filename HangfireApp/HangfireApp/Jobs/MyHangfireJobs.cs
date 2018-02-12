using Microsoft.Extensions.Logging;
using System;

namespace HangfireApp.Jobs
{
    public class MyHangfireJobs
    {
        private readonly ILogger<MyHangfireJobs> _logger;
        public MyHangfireJobs(ILogger<MyHangfireJobs> logger)
        {
            _logger = logger;

            _logger.LogError("我被实例了。");
        }

        public void Run(string msg)
        {
            _logger.LogWarning(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $"{msg}被执行啦。");

        }
    }
}
