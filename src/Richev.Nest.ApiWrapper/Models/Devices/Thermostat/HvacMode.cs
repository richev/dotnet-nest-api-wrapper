using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Richev.Nest.ApiWrapper.Models.Devices.Thermostat
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HvacMode
    {
        /// <summary>
        /// Heating mode
        /// </summary>
        [EnumMember(Value = "heat")]
        Heat,

        /// <summary>
        /// Cooling mode
        /// </summary>
        [EnumMember(Value = "cool")]
        Cool,

        /// <summary>
        /// Heating and cooling (Heat * Cool)
        /// </summary>
        [EnumMember(Value = "heat-cool")]
        HeatAndCool,

        /// <summary>
        /// System is off
        /// </summary>
        [EnumMember(Value = "off")]
        SystemIsOff
    }
}