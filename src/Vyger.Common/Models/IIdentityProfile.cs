using Newtonsoft.Json;

namespace Vyger.Common.Models
{
    public interface IIdentityProfile
    {
        /// <summary>
        ///
        /// </summary>
        string Email { get; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("name")]
        string Name { get; }
    }
}