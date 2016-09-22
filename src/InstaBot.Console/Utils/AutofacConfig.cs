using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using InstaBot.Console.Manager;
using InstaBot.Console.Task;

namespace InstaBot.Console.Utils
{
    internal class AutofacConfig
    {
        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            //Logger
            //builder.RegisterModule<LoggerModule>();
            builder.RegisterInstance(new ConfigurationManager()).As<ConfigurationManager>();

            //Manager
            builder.RegisterAssemblyTypes(typeof(UserManager).Assembly)
               .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(IBaseManager))))
               .PropertiesAutowired()
               .AsImplementedInterfaces();

            //Task
            builder.RegisterAssemblyTypes(typeof(LoginTask).Assembly)
               .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(ITask))))
               .PropertiesAutowired()
               .AsImplementedInterfaces();

            builder.RegisterType<InstaBot>().As<IInstaBot>();
            //Build container
            var container = builder.Build();

            return container;
        }
    }
}
