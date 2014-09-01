using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richev.Nest.ApiWrapper.Models.Structure
{
    /// <summary>
    /// <para>Represents a structure.</para>
    /// <para>For more information, see https://developer.nest.com/documentation/api-reference. </para>
    /// </summary>
    public class NestStructureModel
    {
        /// <summary>
        /// Unique structure identifier.
        /// </summary>
        [JsonProperty("structure_id")]
        public string StructureId { get; set; }

        /// <summary>
        /// List of Thermostats in the structure, by unique identifier.
        /// </summary>
        [JsonProperty("thermostats")]
        public List<string> Thermostats { get; set; }

        /// <summary>
        /// List of smoke+CO alarms in the structure, by unique identifier.
        /// </summary>
        [JsonProperty("smoke_co_alarms")]
        public List<string> Protects { get; set; }

        /// <summary>
        /// Structure state; in order for a structure to enter the 'Auto-Away' state, all devices must be in 'Auto-Away' state;
        /// if any device leaves the 'Auto-Away' state, then the structure also leaves the 'Auto-Away' state.
        /// </summary>
        [JsonProperty("away")]
        public AwayState AwayState { get; set; }

        /// <summary>
        /// User-defined structure name; defaults to 'Home' if the structure type is 'home'.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Country, in ISO 3166-1 alpha-2 format.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Start time of Rush Hour Rewards energy event.
        /// </summary>
        [JsonProperty("peak_period_start_time")]
        public DateTime? PeakPeriodStartTime { get; set; }

        /// <summary>
        /// End time of Rush Hour Rewards energy event.
        /// </summary>
        [JsonProperty("peak_period_end_time")]
        public DateTime? PeakPeriodEndTime { get; set; }

        /// <summary>
        /// Time zone of the structure, in IANA time zone format.
        /// </summary>
        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }
    }
}
