using System;
using Newtonsoft.Json;

namespace Richev.Nest.ApiWrapper.Models.Devices.Thermostat
{
    /// <summary>
    /// <para>Represents a Nest Learning Thermostat.</para>
    /// <para>For more information, see https://developer.nest.com/documentation/api-reference. </para>
    /// </summary>
    public class ThermostatModel : DeviceModelBase
    {
        /// <summary>
        /// System ability to cool (AC).
        /// </summary>
        [JsonProperty("can_cool")]
        public bool CanCool { get; set; }

        /// <summary>
        /// System ability to heat.
        /// </summary>
        [JsonProperty("can_heat")]
        public bool CanHeat { get; set; }

        /// <summary>
        /// Emergency Heat status in systems with heat pumps.
        /// </summary>
        [JsonProperty("is_using_emergency_heat")]
        public bool IsUsingEmergencyHeat { get; set; }

        /// <summary>
        /// System ability to control the fan separately from heating or cooling.
        /// </summary>
        [JsonProperty("has_fan")]
        public bool HasFan { get; set; }

        /// <summary>
        /// Indicates if the fan timer is engaged; used with 'fan_timer_timeout' to turn on the fan for a (user-specified) preset duration.
        /// </summary>
        [JsonProperty("fan_timer_active")]
        public bool FanTimerActive { get; set; }

        /// <summary>
        /// Timestamp, showing when the fan timer reaches 0 (end of timer duration).
        /// </summary>
        [JsonProperty("fan_timer_timeout")]
        public DateTime? FanTimerTimeout { get; set; }

        /// <summary>
        /// Displayed when users choose an energy-saving temperature.
        /// </summary>
        [JsonProperty("has_leaf")]
        public bool HasLeaf { get; set; }

        /// <summary>
        /// Celsius or Fahrenheit; used with temperature display.
        /// </summary>
        [JsonProperty("temperature_scale")]
        public TemperatureScale TemperatureScale { get; set; }

        /// <summary>
        /// Desired temperature, displayed in whole degrees Fahrenheit (1°F).
        /// </summary>
        [JsonProperty("target_temperature_f")]
        public int TargetTemperatureF { get; set; }

        /// <summary>
        /// Desired temperature, displayed in half degrees Celsius (0.5°C).
        /// </summary>
        [JsonProperty("target_temperature_c")]
        public decimal TargetTemperatureC { get; set; }

        /// <summary>
        /// Maximum target temperature, displayed in whole degrees Fahrenheit (1°F); used with Heat • Cool mode.
        /// </summary>
        [JsonProperty("target_temperature_high_f")]
        public int TargetTemperatureHighF { get; set; }

        [JsonProperty("target_temperature_high_c")]
        public decimal TargetTemperatureHighC { get; set; }

        [JsonProperty("target_temperature_low_f")]
        public int TargetTemperatureLowF { get; set; }

        [JsonProperty("target_temperature_low_c")]
        public decimal TargetTemperatureLowC { get; set; }

        [JsonProperty("away_temperature_high_f")]
        public int AwayTemperatureHighF { get; set; }

        [JsonProperty("away_temperature_high_c")]
        public decimal AwayTemperatureHighC { get; set; }

        [JsonProperty("away_temperature_low_f")]
        public int AwayTemperatureLowF { get; set; }

        [JsonProperty("away_temperature_low_c")]
        public decimal AwayTemperatureLowC { get; set; }

        [JsonProperty("hvac_mode")]
        public HvacMode HvacMode { get; set; }

        [JsonProperty("ambient_temperature_f")]
        public int AmbientTemperatureF { get; set; }

        [JsonProperty("ambient_temperature_c")]
        public decimal AmbientTemperatureC { get; set; }
    }
}