using System.Collections.Generic;
using Richev.Nest.ApiWrapper.Models.Devices;
using Richev.Nest.ApiWrapper.Models.Structure;

namespace Richev.Nest.ApiWrapper.Models
{
    /// <summary>
    /// Represents all of the information returned from the Nest API.
    /// </summary>
    public class NestModel
    {
        /// <summary>
        /// Whether OAuth authorization succeeded or failed.
        /// </summary>
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// The devices returned from the Nest API.
        /// </summary>
        public NestDevicesModel Devices { get; set; }

        /// <summary>
        /// The structures returned from the Nest API.
        /// </summary>
        public Dictionary<string, NestStructureModel> Structures { get; set; }
    }
}
