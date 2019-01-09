using Newtonsoft.Json;

namespace vyger.Core.Models
{
    public class GoogleToken
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
    }
}
