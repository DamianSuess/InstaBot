using Newtonsoft.Json;

namespace InstaBot.Console.Model
{
    public class BaseResponseMessage : BaseMessage
    {
        private const string IsOk = "ok";
        private const string IsFail = "fail";

        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

        public bool Success { get { return Status.Equals(IsOk); } }


    }
}