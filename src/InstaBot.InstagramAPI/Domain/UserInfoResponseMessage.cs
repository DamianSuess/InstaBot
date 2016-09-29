using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstaBot.InstagramAPI.Domain
{
    public class UserInfoResponseMessage : BaseResponseMessage
    {
        [JsonProperty("user")]
        public UserInfo User { get; set; }

        public class UserInfo
        {
            public UserInfo()
            {
                ExternalLynxUrl = string.Empty;
                CantBoostPost = false;
                CanSeeOrganicInsights = false;
                ShowInsightsTerms = false;
            }

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
            [JsonProperty("usertags_count")]
            public int UserTagCount { get; set; }
            [JsonProperty("following_count")]
            public int FollowingCount { get; set; }
            [JsonProperty("follower_count")]
            public int FollowerCount { get; set; }
            [JsonProperty("auto_expand_chaining")]
            public string AutoExpandChaining { get; set; }
            [JsonProperty("external_lynx_url")]
            public string ExternalLynxUrl { get; set; }
            [JsonProperty("can_boost_post")]
            public bool CantBoostPost { get; set; }
            [JsonProperty("hd_profile_pic_versions")]
            public List<Candidate> HdProfilePicVersions { get; set; }
            [JsonProperty("hd_profile_pic_url_info")]
            public Caption HdProfilePicUrlInfo { get; set; }
            [JsonProperty("biography")]
            public string Biography { get; set; }
            [JsonProperty("has_chaining")]
            public bool HasChaining { get; set; }
            [JsonProperty("media_count")]
            public int MediaCount { get; set; }
            [JsonProperty("geo_media_count")]
            public int GeoMediaCount { get; set; }
            [JsonProperty("can_see_organic_insights")]
            public bool CanSeeOrganicInsights { get; set; }
            [JsonProperty("can_convert_to_business")]
            public bool CanConvertToBusiness { get; set; }
            [JsonProperty("is_business")]
            public bool IsBusiness { get; set; }
            [JsonProperty("show_insights_terms")]
            public bool ShowInsightsTerms { get; set; }
            [JsonProperty("usertag_review_enabled")]
            public bool UsertagReviewEnabled { get; set; }
            [JsonProperty("external_url")]
            public string ExternalUrl { get; set; }
        }
        
    }
}