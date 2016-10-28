using System.Data;
using System.Linq;
using Autofac;
using InstaBot.Data.Repository;
using ServiceStack.Data;

namespace InstaBot.Data
{
    public class DataModule : Module
    {
        private readonly string _dataPath;

        public DataModule(string dataPath)
        {
            _dataPath = dataPath;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //Data
            var dbFactory = OrmLiteConfig.GetFactory(_dataPath);
            builder.RegisterInstance(dbFactory).As<IDbConnectionFactory>();
            builder.RegisterInstance(OrmLiteConfig.BuildSession(dbFactory)).As<IDbConnection>();

            //Repository
            builder.RegisterAssemblyTypes(typeof(LikedMediaRepository).Assembly)
                .Where(t =>
                    t.GetInterfaces()
                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)))
                .AsImplementedInterfaces()
                .PropertiesAutowired();
        }
    }
}