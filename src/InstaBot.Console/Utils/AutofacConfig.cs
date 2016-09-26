using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using InstaBot.Console.Manager;
using InstaBot.Console.Task;
using ServiceStack.Data;

namespace InstaBot.Console.Utils
{
    internal class AutofacConfig
    {
        public static IContainer ConfigureContainer(string path)
        {
            var builder = new ContainerBuilder();

            //Logger
            builder.RegisterModule<LoggerModule>();
            var dbFactory = OrmLiteConfig.GetFactory(path);
            builder.RegisterInstance(dbFactory).As<IDbConnectionFactory>();
            builder.RegisterInstance(OrmLiteConfig.BuildSession(dbFactory)).As<IDbConnection>();
            builder.RegisterInstance(new ConfigurationManager()).As<ConfigurationManager>();

            //Manager
            builder.RegisterAssemblyTypes(typeof(AccountManager).Assembly)
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
