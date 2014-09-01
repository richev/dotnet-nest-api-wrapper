dotnet-nest-api-wrapper
=======================

Easy-to-use .NET wrapper for the official Nest API.

```c
private readonly string _oauthToken = "get-this-from-nest";
private readonly NestWrapper _nestWrapper = new NestWrapper(new NetGetter());
    
// Call devices.json and structures.json, returning information in a strongly-typed object
// The information returned depends on the permissions requested by the Nest client
// that this OAuth token relates to.
var model = _nestWrapper.GetNestModel(_oauthToken, new TimeSpan(0, 0, 20));

if (model.IsAuthorized)
{
    // authorization was successful, and the model will contain lots of Nest goodies!
}

// If you have a thermostat, you can set its temperature
_nestWrapper.SetThermostatTargetTemperature(_oauthToken, thermostat.DeviceId, thermostat.TemperatureScale, targetTemperature);

// Given a structure, you can set its away mode
_nestWrapper.SetAway(_oauthToken, structure.DeviceId, AwayState.Away);
```

Pre-requisites
--------------
* Register [your own Nest client](https://developer.nest.com/clients)
* Follow the [Nest instructions](https://developer.nest.com/documentation/how-to-auth) on how to get an OAuth token

Notes
-----
* The data structure maps closely to that in the [API reference](https://developer.nest.com/documentation/api-reference)
* Correctly handles [307 redirects from the Nest API](https://developer.nest.com/documentation/data-rate-limits)
* Setting away mode isn't yet working
* Lots more still to be done!
