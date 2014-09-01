using Newtonsoft.Json;

namespace Richev.Nest.ApiWrapper.Models.Devices.Protect
{
    /// <summary>
    /// <para>Represents a Nest Protect smoke and carbon monoxide alarm.</para>
    /// <para>For more information, see https://developer.nest.com/documentation/api-reference. </para>
    /// </summary>
    public class ProtectModel : DeviceModelBase
    {
        /// <summary>
        /// Battery life/health; estimate of time to end of life.
        /// </summary>
        [JsonProperty("battery_health")]
        public BatteryHealth BatteryHealth { get; set; }

        /// <summary>
        /// CO alarm status.
        /// </summary>
        [JsonProperty("co_alarm_state")]
        public AlarmState COAlarmState { get; set; }

        /// <summary>
        /// Smoke alarm status
        /// </summary>
        [JsonProperty("smoke_alarm_state")]
        public AlarmState SmokeAlarmState { get; set; }

        /// <summary>
        /// Indicates device status by color in the Nest app UI.
        /// </summary>
        [JsonProperty("ui_color_state")]
        public UIColorState UIColorState { get; set; }
    }
}