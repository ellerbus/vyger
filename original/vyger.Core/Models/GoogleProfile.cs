using Newtonsoft.Json;

namespace vyger.Core.Models
{
    public class GoogleProfile
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("verified_email")]
        public bool VerifiedEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("link")]
        public string Link { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("picture")]
        public string Picture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("locale")]
        public string Locale { get; set; }
    }
}
