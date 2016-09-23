using Newtonsoft.Json;

namespace InstaBot.Console.Model
{
    public class User
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
        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }
        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }
        [JsonProperty("is_unpublished")]
        public bool? IsUnpublished { get; set; }
        [JsonProperty("coeff_weight")]
        public bool CoeffWeight { get; set; }
        [JsonProperty("friendship_status")]
        public FriendshipStatus FriendshipStatus { get; set; }

    }
}