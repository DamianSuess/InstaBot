using System.IO;
using InstaBot.Console.Settings;
using InstaBot.InstagramAPI.Settings;

namespace InstaBot.Console
{
    public class ConfigurationManager
    {
        private AuthSettings _authSettings = new AuthSettings();
        private ApiSettings _apiSettings = new ApiSettings();
        private BotSettings _botSettings = new BotSettings();

        private string _path;

        public ConfigurationManager(string path)
        {
            _path = path;
        }

        public void Load()
        {
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), _path);
            var profileConfigPath = Path.Combine(profilePath, "config");
            LoadAuthSettings(profileConfigPath);
            LoadApiSettings(profileConfigPath);
            LoadBotSettings(profileConfigPath);
        }

        public void LoadApiSettings(string path)
        {
            var configFile = Path.Combine(path, "api.json");
            _apiSettings.Load(configFile);
        }

        public void LoadAuthSettings(string path)
        {
            var configFile = Path.Combine(path, "auth.json");
            _authSettings.Load(configFile);
        }

        public void LoadBotSettings(string path)
        {
            var configFile = Path.Combine(path, "bot.json");
            _botSettings.Load(configFile);
        }

        public IAuthSettings AuthSettings { get { return _authSettings; } }
        public IApiSettings ApiSettings { get { return _apiSettings; } }
        public BotSettings BotSettings { get { return _botSettings; } }

    }
}