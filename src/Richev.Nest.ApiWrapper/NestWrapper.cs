using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Richev.Nest.ApiWrapper.Models;
using Richev.Nest.ApiWrapper.Models.Devices;
using Richev.Nest.ApiWrapper.Models.Devices.Protect;
using Richev.Nest.ApiWrapper.Models.Devices.Thermostat;
using Richev.Nest.ApiWrapper.Models.Structure;
using Richev.Nest.ApiWrapper.Net;

namespace Richev.Nest.ApiWrapper
{
    /// <summary>
    /// <para>Wraps functionality provided by the Nest API.</para>
    /// <para>API documentation can be found at https://developer.nest.com/documentation/api-reference. </para>
    /// </summary>
    public class NestWrapper
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(NestWrapper));

        private readonly INetGetter _netGetter;

        private readonly Session _session = new Session();

        /// <summary>
        /// Stores information that is needed between calls to the Nest API.
        /// </summary>
        private class Session 
        {
            /// <summary>
            /// <para>The OAuth access token.</para>
            /// </summary>
            public string AccessToken { get; set; }

            /// <summary>
            /// The URL to use for subsequent calls to devices.json (given in a 307 redirect).
            /// </summary>
            public string DevicesSubsequentUrl { get; set; }

            /// <summary>
            /// The URL to use for subsequent calls to structures.json (given in a 307 redirect).
            /// </summary>
            public string StructuresSubsequentUrl { get; set; }

            /// <summary>
            /// <para>The URL to use for subsequent calls set the away state (given in a 307 redirect).</para>
            /// <para>Keyed by Structure Id.</para>
            /// </summary>
            public Dictionary<string, string> AwaySetSubsequentUrl { get; set; }

            /// <summary>
            /// <para>The URL to use for subsequent calls to set the thermostat temperature (given in a 307 redirect).</para>
            /// <para>Keyed by thermostat DeviceId.</para>
            /// </summary>
            public Dictionary<string, string> TemperatureSetSubsequentUrl { get; set; }

            /// <summary>
            /// <para>Keyed by thermostat DeviceId.</para>
            /// </summary>
            public Dictionary<string, ThermostatModel> PreviousThermostats { get; set; }
        }

        public delegate void ThermostatStateChangedEventHandler(object sender, ThermostatStateEventArgs e);

        /// <summary>
        /// Event that fires when the heating or leaf state of a thermostat changes.
        /// </summary>
        public event ThermostatStateChangedEventHandler ThermostatStateChanged;

        public NestWrapper(INetGetter netGetter)
        {
            _netGetter = netGetter;
            InitSession();
        }

        /// <summary>
        /// Calls the Nest API to get devices and structures (as permitted by the Nest client referenced by the given access token),
        /// returning the information in one strongly-typed object.
        /// </summary>
        /// <param name="accessToken">OAuth access token.</param>
        /// <param name="timeout">Timeout for the HTTP calls to the Nest API.</param>
        public NestModel GetNestModel(string accessToken, TimeSpan timeout)
        {
            var isAuthorized = true;
            NestDevicesModel devices = null;
            Dictionary<string, NestStructureModel> structures = null;

            if (string.IsNullOrEmpty(accessToken))
            {
                return new NestModel { IsAuthorized = false };
            }

            if (accessToken != _session.AccessToken)
            {
                InitSession();
            }

            _session.AccessToken = accessToken;

            try
            {
                Parallel.Invoke(
                    () => devices = GetDevices(accessToken, timeout),
                    () => structures = GetStructures(accessToken, timeout));
            }
            catch (AggregateException ex)
            {
                _log.Error(ex);
                _log.Error(ex.InnerException);

                if (ex.InnerException is WebException && ex.InnerException.Message.Contains("(401)"))
                {
                    if (_log.IsInfoEnabled)
                    {
                        _log.Info("User is not authorized.");
                    }
                    isAuthorized = false;
                }
                else
                {
                    throw ex.InnerException;
                }
            }

            if (devices != null)
            {
                FireHeatingStateChangedEvents(devices.Thermostats, AwayState.Home);
                _session.PreviousThermostats = devices.Thermostats;
            }

            return new NestModel { IsAuthorized = isAuthorized, Devices = devices, Structures = structures };
        }

        private void FireHeatingStateChangedEvents(Dictionary<string, ThermostatModel> thermostats, AwayState awayState)
        {
            if (ThermostatStateChanged == null) return;

            foreach (var thermostat in thermostats)
            {
                if (_session.PreviousThermostats.ContainsKey(thermostat.Value.DeviceId))
                {
                    var previousThermostat = _session.PreviousThermostats[thermostat.Value.DeviceId];
                    var currentHeatingState = thermostat.Value.GetHeatingState(awayState);

                    if (previousThermostat.GetHeatingState(awayState) != currentHeatingState ||
                        previousThermostat.HasLeaf != thermostat.Value.HasLeaf)
                    {
                        ThermostatStateChanged(this, new ThermostatStateEventArgs
                                                     {
                                                         DeviceId = thermostat.Value.DeviceId,
                                                         Previous = new ThermostatState
                                                                    {
                                                                        Heating = previousThermostat.GetHeatingState(awayState),
                                                                        HasLeaf = previousThermostat.HasLeaf
                                                                    },
                                                         Current = new ThermostatState
                                                               {
                                                                   Heating = currentHeatingState,
                                                                   HasLeaf = thermostat.Value.HasLeaf
                                                               }
                                                     });
                    }
                }
            }
        }

        private NestDevicesModel GetDevices(string accessToken, TimeSpan timeout)
        {
            string subsequentRequestsUrl;

            var url = string.IsNullOrEmpty(_session.DevicesSubsequentUrl) ?
                string.Format("https://developer-api.nest.com/devices.json?auth={0}", accessToken) :
                _session.DevicesSubsequentUrl;

            var devicesJson = GetJsonObj(url, timeout, out subsequentRequestsUrl);

            if (_log.IsInfoEnabled && _session.DevicesSubsequentUrl != subsequentRequestsUrl)
            {
                _log.InfoFormat("Got devices using '{0}', subsequent requests will use '{1}'.", url, subsequentRequestsUrl);
            }

            _session.DevicesSubsequentUrl = subsequentRequestsUrl;

            var devices = JsonConvert.DeserializeObject<NestDevicesModel>(devicesJson.ToString());

            if (devices.Thermostats == null)
            {
                devices.Thermostats = new Dictionary<string, ThermostatModel>();
            }

            if (devices.Protects == null)
            {
                devices.Protects = new Dictionary<string, ProtectModel>();
            }

            if (_log.IsInfoEnabled)
            {
                _log.InfoFormat(
                    "Received data for {0} Thermostat(s) ({1}) and {2} Protect(s) ({3}).",
                    devices.Thermostats.Count,
                    string.Join(", ", devices.Thermostats.Select(t => t.Value.Name)),
                    devices.Protects.Count,
                    string.Join(", ", devices.Protects.Select(t => t.Value.Name)));
            }

            return devices;
        }

        private Dictionary<string, NestStructureModel> GetStructures(string accessToken, TimeSpan timeout)
        {
            string subsequentRequestsUrl;

            var url = string.IsNullOrEmpty(_session.StructuresSubsequentUrl) ?
                string.Format("https://developer-api.nest.com/structures.json?auth={0}", accessToken) :
                _session.StructuresSubsequentUrl;

            var structuresJson = GetJsonObj(url, timeout, out subsequentRequestsUrl);

            if (_log.IsInfoEnabled && _session.StructuresSubsequentUrl != subsequentRequestsUrl)
            {
                _log.InfoFormat("Got structures using '{0}', subsequent requests will use '{1}'.", url, subsequentRequestsUrl);
            }

            _session.StructuresSubsequentUrl = subsequentRequestsUrl;

            var structures = JsonConvert.DeserializeObject<Dictionary<string, NestStructureModel>>(structuresJson.ToString());

            if (_log.IsInfoEnabled)
            {
                _log.InfoFormat(
                    "Received data for {0} structure(s) ({1}).",
                    structures.Count,
                    string.Join(", ", structures.Values.Select(s => s.Name)));
            }

            return structures;
        }

        private JObject GetJsonObj(string url, TimeSpan timeout, out string subsequentRequestsUrl)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = (int)timeout.TotalMilliseconds;

            var responseJson = _netGetter.GetResponseText(request, out subsequentRequestsUrl);

            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("Response received from '{0}':{1}", url, responseJson);
            }

            var responseObj = (JObject)JsonConvert.DeserializeObject(responseJson);

            return responseObj;
        }

        /// <summary>
        /// Sets the target temperature of the thermostat to the given value.
        /// </summary>
        /// <param name="accessToken">OAuth access token.</param>
        /// <param name="thermostatDeviceId">The DeviceId of the thermostat.</param>
        /// <param name="thermostatTemperatureScale">The temperature scale of the target temperature.</param>
        /// <param name="targetTemperature">The target temperature, in °C or °F depending on the temperature scale.</param>
        public void SetThermostatTargetTemperature(string accessToken, string thermostatDeviceId, TemperatureScale thermostatTemperatureScale, decimal targetTemperature)
        {
            if (_log.IsInfoEnabled)
            {
                _log.InfoFormat("Setting thermostat '{0}' to {1} {2}.", thermostatDeviceId, targetTemperature, thermostatTemperatureScale);
            }

            string setTargetTemperatureUrl;

            if (_session.TemperatureSetSubsequentUrl.ContainsKey(thermostatDeviceId) && _session.PreviousThermostats[thermostatDeviceId].TemperatureScale == thermostatTemperatureScale)
            {
                setTargetTemperatureUrl = _session.TemperatureSetSubsequentUrl[thermostatDeviceId];
            }
            else
            {
                setTargetTemperatureUrl = string.Format(
                    "https://developer-api.nest.com/devices/thermostats/{0}/target_temperature_{1}?auth={2}",
                    thermostatDeviceId,
                    Utils.ToEnumString(thermostatTemperatureScale).ToLower(),
                    accessToken);
            }

            var targetTemperatureString = thermostatTemperatureScale == TemperatureScale.Celcuis ? targetTemperature.ToString("0.0") : targetTemperature.ToString();
            string subsequentTemperatureSetUrl;

            SetNewValue(setTargetTemperatureUrl, targetTemperatureString, out subsequentTemperatureSetUrl);

            _session.TemperatureSetSubsequentUrl[thermostatDeviceId] = subsequentTemperatureSetUrl;
        }

        public void SetAway(string accessToken, string structureId, AwayState awayState)
        {
            if (_log.IsInfoEnabled)
            {
                _log.InfoFormat("Setting away state of structure '{0}' to {1}.", structureId, awayState);
            }

            var setAwayUrl = string.IsNullOrEmpty(_session.AwaySetSubsequentUrl[structureId]) ?
                string.Format("https://developer-api.nest.com/devices/structures/{0}/away?auth={1}", structureId, accessToken) :
                _session.AwaySetSubsequentUrl[structureId];

            string subsequentAwaySetUrl;

            SetNewValue(setAwayUrl, "\"" + Utils.ToEnumString(awayState) + "\"", out subsequentAwaySetUrl);

            _session.AwaySetSubsequentUrl[structureId] = subsequentAwaySetUrl;
        }

        private void SetNewValue(string setUrl, string setValue, out string subsequentUri)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(setUrl));
            request.Method = "PUT";
            request.ContentType = "application/json";

            var bytes = Encoding.Default.GetBytes(setValue);
            request.ContentLength = bytes.Length;

            using (var writeStream = request.GetRequestStream())
            {
                writeStream.Write(bytes, 0, bytes.Length);
            }

            var responseText = _netGetter.GetResponseText(request, out subsequentUri);

            if (setValue != responseText)
            {
                throw new WebException(string.Format("The response was expected to be '{0}', but was '{1}'.", setValue, responseText));
            }
        }

        /// <summary>
        /// <para>
        /// When the user authorizes, any previously-saved URLs become invalid (because we have a new access token and this forms part of the URL).
        /// </para>
        /// <para>
        /// If the user revoked and re-granted access to this app while it was running, then we have an edge case in which
        /// these URLs must be cleared from session because they will no longer work.
        /// </para>
        /// </summary>
        private void InitSession()
        {
            _session.DevicesSubsequentUrl = string.Empty;
            _session.StructuresSubsequentUrl = string.Empty;
            _session.AwaySetSubsequentUrl = new Dictionary<string, string>();
            _session.TemperatureSetSubsequentUrl = new Dictionary<string, string>();
            _session.PreviousThermostats = new Dictionary<string, ThermostatModel>();
        }
    }
}