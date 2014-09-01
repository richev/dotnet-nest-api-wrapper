using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Richev.Nest.ApiWrapper.Models.Structure
{
    /// <summary>
    /// Structure state; in order for a structure to enter the 'Auto-Away' state, all devices must be in 'Auto-Away' state;
    /// if any device leaves the 'Auto-Away' state, then the structure also leaves the 'Auto-Away' state.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AwayState
    {
        /// <summary>
        /// Home state.
        /// </summary>
        [EnumMember(Value = "home")]
        Home,

        /// <summary>
        /// Away state.
        /// </summary>
        [EnumMember(Value = "away")]
        Away,

        /// <summary>
        /// Auto-Away state.
        /// </summary>
        [EnumMember(Value = "auto-away")]
        AutoAway
    }
}