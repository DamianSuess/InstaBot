using System.Data;
using System.IO;
using InstaBot.Core.Domain;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace InstaBot.Data
{
    public static class OrmLiteConfig
    {
        public static string SqliteFileDb = "db.sqlite".MapAbsolutePath();

        public static IDbConnectionFactory GetFactory(string path)
        {
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), path);
            var profileConfigPath = Path.Combine(profilePath, "config");
            var dbPath = Path.Combine(profileConfigPath, SqliteFileDb);
            var dbFactory = new OrmLiteConnectionFactory(dbPath, SqliteDialect.Provider);
            return dbFactory;
        }

        public static IDbConnection BuildSession(IDbConnectionFactory factory)
        {
            var session =  factory.Open();
            session.CreateTableIfNotExists<LikedMedia>();
            session.CreateTableIfNotExists<FollowedUser>();
            return session;
        }

    }
}
