namespace Richev.Nest.ApiWrapper
{
    /// <summary>
    /// Represents simple information about the state of a thermostat.
    /// </summary>
    public class ThermostatState
    {
        /// <summary>
        /// The state of the heating or cooling.
        /// </summary>
        public ThermostatHeatingState Heating { get; set; }

        /// <summary>
        /// The leaf state.
        /// </summary>
        public bool HasLeaf { get; set; }
    }
}
