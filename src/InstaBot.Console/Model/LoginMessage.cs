using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InstaBot.Console.Model
{
    public class LoginMessage : BaseMessage
    {
        public LoginMessage(string token, string login, string password)
        {
            PhoneId = System.Guid.NewGuid().ToString();
            Guid = System.Guid.NewGuid().ToString();
            Token = token;
            Login = login;
            Password = password;
            LoginAttemptCount = "0";
            DeviceId = $"android-{System.Guid.NewGuid().ToString("N").Substring(0,16)}";
        }

        [JsonProperty("phone_id")]
        public string PhoneId { get; set; }
        [JsonProperty("_csrftoken")]
        public string Token { get; set; }
        [JsonProperty("username")]
        public string Login { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("guid")]
        public string Guid { get; set; }
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
        [JsonProperty("login_attempt_count")]
        public string LoginAttemptCount { get; set; }

        public override string ToString()
        {
            var jsonSerializeSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.None,
                Converters = new JsonConverter[] { new StringEnumConverter { CamelCaseText = true } }
            };

            var output = JsonConvert.SerializeObject(this, jsonSerializeSettings);
            return output;

        }
    }
}
