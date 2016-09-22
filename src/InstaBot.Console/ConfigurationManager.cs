using System.IO;
using InstaBot.Console.Settings;

namespace InstaBot.Console
{
    public class ConfigurationManager
    {
        private AuthSettings _authSettings = new AuthSettings();
        private ApiSettings _apiSettings = new ApiSettings();

        public void Load(string path)
        {

            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), path);
            var profileConfigPath = Path.Combine(profilePath, "config");
            LoadAuthSettings(profileConfigPath);
            LoadApiSettings(profileConfigPath);
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

        public AuthSettings AuthSettings { get { return _authSettings; } }
        public ApiSettings ApiSettings { get { return _apiSettings; } }

    }
}