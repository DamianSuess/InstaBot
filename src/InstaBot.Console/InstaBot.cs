using System;
using System.Threading.Tasks;
using InstaBot.Console.Tasks;
using InstaBot.Logging;

namespace InstaBot.Console
{
    public interface IInstaBot
    {
        void Run();
    }

    public class InstaBot : IInstaBot
    {
        public ConfigurationManager ConfigurationManager { get; set; }
        public ILogger Logger { get; set; }
        public ILogin LoginTask { get; set; }
        public ILikeTask LikeTask { get; set; }
        public IFollowingTask FollowingTask { get; set; }


        public void Run()
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            CheckConfiguration();
            LoginTask.DoLogin().Wait();
            LikeTask.Start();
            FollowingTask.Start();
        }

        private void CheckConfiguration()
        {
            if (!ConfigurationManager.IsAuthValid)
            {
                var message = "Please configure auth.json";
                Logger.Critical(message);
                throw new Exception(message);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Logger.Critical("Fatal error occured in task", e.Exception);
            throw new Exception("Fatal error occured in sub task", e.Exception);
        }
    }
}