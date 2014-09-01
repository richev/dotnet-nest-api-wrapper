using Newtonsoft.Json;

namespace Richev.Nest.ApiWrapper.Models
{
    /// <summary>
    /// Represents an error returned from the Nest API
    /// </summary>
    internal class NestErrorResponseModel
    {
        /// <summary>
        /// The message of the error.
        /// </summary>
        [JsonProperty("error")]
        public string Message { get; set; }
    }
}
