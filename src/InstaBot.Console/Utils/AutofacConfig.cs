using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using InstaBot.Console.Tasks;
using InstaBot.Data;
using InstaBot.InstagramAPI.Manager;
using InstaBot.InstagramAPI.Settings;
using ServiceStack.Data;
using TinyMessenger;

namespace InstaBot.Console.Utils
{
    internal class AutofacConfig
    {
        public static IContainer ConfigureContainer(string path)
        {
            var builder = new ContainerBuilder();

            //Logger
            builder.RegisterModule<LoggerModule>();

            //Data
            builder.RegisterModule(new DataModule(path));

            //Configuration
            var configurationManager = new ConfigurationManager(path);
            configurationManager.Load();
            builder.RegisterInstance(configurationManager).As<ConfigurationManager>();
            builder.RegisterInstance(configurationManager.ApiSettings).As<IApiSettings>();
            builder.RegisterInstance(configurationManager.AuthSettings).As<IAuthSettings>();

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

            //Messenger
            builder.RegisterInstance(new TinyMessengerHub()).As<ITinyMessengerHub>();

            //Main bot class
            builder.RegisterType<InstaBot>().As<IInstaBot>()
                .PropertiesAutowired();


            //Build container
            var container = builder.Build();

            return container;
        }
    }
}
