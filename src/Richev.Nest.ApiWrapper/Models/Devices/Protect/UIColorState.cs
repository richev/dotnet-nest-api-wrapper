namespace Richev.Nest.ApiWrapper.Models.Devices.Protect
{
    /// <summary>
    /// Device status by color in the Nest app UI.
    /// </summary>
    public enum UIColorState
    {
        /// <summary>
        /// Offline
        /// </summary>
        Gray,

        /// <summary>
        /// OK
        /// </summary>
        Green,

        /// <summary>
        /// Warning
        /// </summary>
        Yellow,

        /// <summary>
        /// Emergency
        /// </summary>
        Red
    }
}