namespace Richev.Nest.ApiWrapper.Models.Devices.Protect
{
    /// <summary>
    /// Alarm status.
    /// </summary>
    public enum AlarmState
    {
        /// <summary>
        /// OK.
        /// </summary>
        Ok,

        /// <summary>
        /// Warning - detected.
        /// </summary>
        Warning,

        /// <summary>
        /// Emergency - detected, move to fresh air.
        /// </summary>
        Emergency
    }
}