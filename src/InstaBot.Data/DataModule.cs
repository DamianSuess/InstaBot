using System.Data;
using Autofac;
using ServiceStack.Data;

namespace InstaBot.Data
{
    public class DataModule : Module
    {
        private string _dataPath;

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
        }
    }
}