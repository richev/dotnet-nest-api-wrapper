using System.Collections.Generic;
using Newtonsoft.Json;
using Richev.Nest.ApiWrapper.Models.Devices.Protect;
using Richev.Nest.ApiWrapper.Models.Devices.Thermostat;

namespace Richev.Nest.ApiWrapper.Models.Devices
{
    /// <summary>
    /// <para>Represents the Nest Thermostats and Nest Protects returned from the Nest API.</para>
    /// <para>For more information, see https://developer.nest.com/documentation/api-reference. </para>
    /// </summary>
    public class NestDevicesModel
    {
        /// <summary>
        /// The Nest Thermostats.
        /// </summary>
        [JsonProperty("thermostats")]
        public Dictionary<string, ThermostatModel> Thermostats { get; set; }

        /// <summary>
        /// The Nest Protects.
        /// </summary>
        [JsonProperty("smoke_co_alarms")]
        public Dictionary<string, ProtectModel> Protects { get; set; }
    }
}