using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Richev.Nest.ApiWrapper.Models.Devices.Thermostat
{
    /// <summary>
    /// Celsius or Fahrenheit; used with temperature display.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TemperatureScale
    {
        [EnumMember(Value = "C")]
        Celcuis,

        [EnumMember(Value = "F")]
        Fahrenheit
    }
}