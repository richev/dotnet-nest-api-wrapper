dotnet-nest-api-wrapper
=======================

Easy-to-use .NET wrapper for the official Nest API.

```c
private readonly NestWrapper _nestWrapper = new NestWrapper(new NetGetter());
    
// Call devices.json and structures.json, returning information in a strongly-typed object
var model = _nestWrapper.GetNestModel("oath-token", new TimeSpan(0, 0, 20));

if (model.IsAuthorized)
{
    // authorization was successful
}

_nestWrapper.SetThermostatTargetTemperature("oauth-token", thermostat.DeviceId, thermostat.TemperatureScale, targetTemperature);

_nestWrapper.SetAway("oauth-token", structure.DeviceId, AwayState.Away);
```

* Correctly handles [307 redirects from the Nest API](https://developer.nest.com/documentation/data-rate-limits)
*	You need to register [your own Nest client](https://developer.nest.com/clients) and then obtain an OAuth access token
