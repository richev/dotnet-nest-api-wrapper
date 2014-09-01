using System;
using Richev.Nest.ApiWrapper.Models.Devices.Thermostat;
using Richev.Nest.ApiWrapper.Models.Structure;

namespace Richev.Nest.ApiWrapper
{
    public static class ThermostatExtensionMethods
    {
        /// <summary>
        /// Gets the current heating state of the thermostat, based on an educated guess (verified by experimentation).
        /// </summary>
        public static ThermostatHeatingState GetHeatingState(this ThermostatModel model, AwayState awayState)
        {
            var state = ThermostatHeatingState.NotHeatingOrCooling;

            if (model.IsOnline)
            {
                switch (model.HvacMode)
                {
                    case HvacMode.SystemIsOff:
                        state = ThermostatHeatingState.Off;
                        break;

                    case HvacMode.HeatAndCool:
                        var targetTemperatureHigh = awayState == AwayState.Home ? model.TargetTemperatureHighF : model.AwayTemperatureHighF;
                        var targetTemperatureLow = awayState == AwayState.Home ? model.TargetTemperatureLowF : model.AwayTemperatureLowF;

                        if (targetTemperatureHigh > model.AmbientTemperatureF)
                        {
                            state = ThermostatHeatingState.Heating; // sort of a guess
                        }
                        else if (targetTemperatureLow < model.AmbientTemperatureF)
                        {
                            state = ThermostatHeatingState.Cooling; // sort of a guess
                        }
                        break;

                    default:
                        var targetTemperature = awayState == AwayState.Home ? model.TargetTemperatureF : model.AwayTemperatureLowF;

                        if (model.CanHeat && targetTemperature > model.AmbientTemperatureF)
                        {
                            state = ThermostatHeatingState.Heating; // sort of a guess
                        }
                        else if (model.CanCool && targetTemperature < model.AmbientTemperatureF)
                        {
                            state = ThermostatHeatingState.Cooling; // sort of a guess
                        }
                        break;
                }
            }
            else
            {
                state = ThermostatHeatingState.Offline;
            }

            return state;
        }

        /// <summary>
        /// Gets the numeric value of the target temperature of the thermostat, given the temperature scale (°C or °F).
        /// </summary>
        public static decimal GetTargetTemperature(this ThermostatModel model)
        {
            return GetTemperature(model, () => model.TargetTemperatureC, () => model.TargetTemperatureF);
        }

        /// <summary>
        /// Gets the numeric value of the ambient temperature reported by the thermostat, given the temperature scale (°C or °F).
        /// </summary>
        public static decimal GetAmbientTemperature(this ThermostatModel model)
        {
            return GetTemperature(model, () => model.AmbientTemperatureC, () => model.AmbientTemperatureF);
        }

        /// <summary>
        /// Gets the numeric value of the away temperature of the thermostat, given the temperature scale (°C or °F).
        /// </summary>
        public static string GetAwayTemperature(this ThermostatModel model)
        {
            return GetDisplayTemperature(model, () => model.AwayTemperatureLowC, () => model.AwayTemperatureLowF); // BUG: What if HVAC?
        }

        /// <summary>
        /// Gets a formatted string for displaying the target temperature of the thermostat, given the temperature scale (°C or °F).
        /// </summary>
        public static string GetDisplayTargetTemperature(this ThermostatModel model)
        {
            return GetDisplayTemperature(model, () => model.TargetTemperatureC, () => model.TargetTemperatureF);
        }

        /// <summary>
        /// Gets a formatted string for displaying the high target temperature of the thermostat, given the temperature scale (°C or °F).
        /// </summary>
        public static string GetDisplayTargetTemperatureHigh(this ThermostatModel model)
        {
            return GetDisplayTemperature(model, () => model.TargetTemperatureHighC, () => model.TargetTemperatureHighF);
        }

        /// <summary>
        /// Gets a formatted string for displaying the low target temperature of the thermostat, given the temperature scale (°C or °F).
        /// </summary>
        public static string GetDisplayTargetTemperatureLow(this ThermostatModel model)
        {
            return GetDisplayTemperature(model, () => model.TargetTemperatureLowC, () => model.TargetTemperatureLowF);
        }

        public static string GetDisplayAwayTemperatureHigh(this ThermostatModel model)
        {
            return GetDisplayTemperature(model, () => model.AwayTemperatureHighC, () => model.AwayTemperatureHighF);
        }

        public static string GetDisplayAwayTemperatureLow(this ThermostatModel model)
        {
            return GetDisplayTemperature(model, () => model.AwayTemperatureLowC, () => model.AwayTemperatureLowF);
        }

        /// <summary>
        /// Gets a formatted string for displaying the ambient temperature reported by the thermostat, given the temperature scale (°C or °F).
        /// </summary>
        public static string GetDisplayAmbientTemperature(this ThermostatModel model)
        {
            return GetDisplayTemperature(model, () => model.AmbientTemperatureC, () => model.AmbientTemperatureF);
        }

        private static decimal GetTemperature(ThermostatModel model, Func<decimal> celciusFunc, Func<int> fahrenheitFunc)
        {
            decimal temperature;

            switch (model.TemperatureScale)
            {
                case TemperatureScale.Celcuis:
                    temperature = celciusFunc();
                    break;

                case TemperatureScale.Fahrenheit:
                    temperature = fahrenheitFunc();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(string.Format("Unknown temperature scale '{0}' encountered.", model.TemperatureScale));
            }

            return temperature;
        }

        private static string GetDisplayTemperature(ThermostatModel model, Func<decimal> celciusFunc, Func<int> fahrenheitFunc)
        {
            var temperature = GetTemperature(model, celciusFunc, fahrenheitFunc);

            return string.Format("{0:0.#}°{1}", temperature, Utils.ToEnumString(model.TemperatureScale));
        }
    }
}
