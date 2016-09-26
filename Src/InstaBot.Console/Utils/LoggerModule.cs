using Autofac;
using InstaBot.Logging;

namespace InstaBot.Console.Utils
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            LogManager.UseConsole();
            builder.RegisterInstance(LogManager.GetLogger("InstaBot")).As<ILogger>();
        }
    }
}