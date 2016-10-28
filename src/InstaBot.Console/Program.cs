using System.Linq;
using Autofac;
using InstaBot.Console.Utils;
using InstaBot.Logging;

namespace InstaBot.Console
{
    internal class Program
    {
        private static IContainer Container { get; set; }

        //https://github.com/mgp25/Instagram-API/tree/master/src
        private static void Main(string[] args)
        {
            Container = AutofacConfig.ConfigureContainer(args.Any() && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : string.Empty);

            using (var scope = Container.BeginLifetimeScope())
            {
                var logger = scope.Resolve<ILogger>();
                logger.Trace("### Start IntaBot ###");
                var bot = scope.Resolve<IInstaBot>();
                bot.Run();
            }

            System.Console.ReadLine();
        }
    }
}