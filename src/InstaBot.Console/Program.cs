using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using InstaBot.Console.Settings;
using InstaBot.Console.Task;
using InstaBot.Console.Utils;

namespace InstaBot.Console
{
    class Program
    {
        private static IContainer Container { get; set; }

        //https://github.com/mgp25/Instagram-API/tree/master/src
        static void Main(string[] args)
        {
            Container = AutofacConfig.ConfigureContainer();

            using (var scope = Container.BeginLifetimeScope())
            {
                var bot = scope.Resolve<IInstaBot>();
                bot.Run();
            }
        }
    }
}
