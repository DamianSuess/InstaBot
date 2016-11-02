using System;
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
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            Initalize(args);
            Start();

            System.Console.ReadLine();
            Environment.Exit(0);
        }
        private static void Initalize(string[] args)
        {
            Container =
                AutofacConfig.ConfigureContainer(args.Any() && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : string.Empty);
        }

        private static void Start()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var logger = scope.Resolve<ILogger>();
                logger.Trace("### Start IntaBot ###");
                var bot = scope.Resolve<IInstaBot>();
                bot.Run();
            }
        }


        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                using (var scope = Container.BeginLifetimeScope())
                {
                    var logger = scope.Resolve<ILogger>();
                    var exception = e.ExceptionObject as Exception;
                    if (exception != null)
                        logger.Critical(exception);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }

            System.Console.WriteLine(e.ExceptionObject.ToString());
            System.Console.WriteLine("Press Enter to continue");
            System.Console.ReadLine();
            Environment.Exit(1);
        }
    }
}