using System;
using Newtonsoft.Json;

namespace Richev.Nest.ApiWrapper.Models.Devices
{
    /// <summary>
    /// <para>Base class for Nest devices (Thermostats and Protects).</para>
    /// <para>For more information, see https://developer.nest.com/documentation/api-reference. </para>
    /// </summary>
    public abstract class DeviceModelBase
    {
        /// <summary>
        /// Unique identifier for the device.
        /// </summary>
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        /// <summary>
        /// Country and language preference, in IETF Language Tag format.
        /// </summary>
        [JsonProperty("locale")]
        public string Locale { get; set; }

        /// <summary>
        /// Software version.
        /// </summary>
        [JsonProperty("software_version")]
        public string SoftwareVersion { get; set; }

        /// <summary>
        /// The structure to which the device belongs.
        /// </summary>
        [JsonProperty("structure_id")]
        public string StructureId { get; set; }

        /// <summary>
        /// Display name of the device.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Long display name of the device.
        /// </summary>
        [JsonProperty("name_long")]
        public string NameLong { get; set; }

        /// <summary>
        /// Time of the last cussessful interaction with the Nest Service.
        /// </summary>
        [JsonProperty("last_connection")]
        public DateTime LastConnection { get; set; }

        /// <summary>
        /// Device connection status with the Nest Service.
        /// </summary>
        [JsonProperty("is_online")]
        public bool IsOnline { get; set; }
    }
}
