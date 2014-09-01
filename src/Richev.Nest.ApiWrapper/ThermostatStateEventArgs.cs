namespace Richev.Nest.ApiWrapper
{
    public class ThermostatStateEventArgs
    {
        /// <summary>
        /// The unique identifier of this thermostat.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// The state of this thermostat when it was previously checked.
        /// </summary>
        public ThermostatState Previous { get; set; }

        /// <summary>
        /// The current state of this thermostat.
        /// </summary>
        public ThermostatState Current { get; set; }
    }
}
