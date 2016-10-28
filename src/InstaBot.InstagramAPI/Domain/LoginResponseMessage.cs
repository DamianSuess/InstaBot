using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class LoginResponseMessage : BaseResponseMessage
    {
        public class LoggedUser
        {
            [JsonProperty("pk")]
            public string Id { get; set; }
            [JsonProperty("username")]
            public string UserName { get; set; }
            [JsonProperty("full_name")]
            public string FullName { get; set; }
            [JsonProperty("has_anonymous_profile_picture")]
            public bool IsAnonymousProfilePicture { get; set; }
            [JsonProperty("profile_pic_url")]
            public string PictureUrl { get; set; }
            [JsonProperty("profile_pic_id")]
            public string PictureId { get; set; }
            [JsonProperty("is_private")]
            public bool IsPrivate { get; set; }
        }

        [JsonProperty("logged_in_user")]
        public LoggedUser LoggedInUser { get; set; }
    }
}
