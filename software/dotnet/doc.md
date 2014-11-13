SDShare Push - .NET implementation
==================================

Features
--------

* Pluggable and configurable abstraction *IFragmentReceiver*
* Configurable support for idempotency
* Support for simple hosting using ASP.NET Web API and OWIN

IFragmentReceiver
-----------------
The core functionalty of the framework is to provide the abstraction *SdShare.IFragmentReceiver* as well as configuration functionality. In other words, you as a developer utilise this framework by writing classes that implement *IFragmentReceiver*, and then plugging them into a host through configuration.

Projects and assemblies
-----------------------
The functionality is divided into two separate assemblies:

* __SdShare.Core.dll__ *.\software\dotnet\SdSharePushReceiver\Core\* Contains all functionality apart from hosting.
* __SdShare.Service.AspNetWebApi.dll__ *.\software\dotnet\SdSharePushReceiver\Service.AspNetWebApi\* Provides hosting functionality by utilising ASP.NET Web API and OWIN. Be aware that this is not an executable host, but rather classes that can easily incorporated into a hosting application (for instance a windows service application).

Configuration
-------------
The functionality is configured via standard xml configuration that is localised in the host's application configuration file. The following xml snippet is valid configuration:

```xml
<configuration>
    <configSections>
        <section 
            name="SdShareReceiverConfigurationSection" 
            type="SdShare.Configuration.SdShareReceiverConfigurationSection, SdShare.Core"/>
    </configSections>

    <SdShareReceiverConfigurationSection port="9001">
        <Receivers>
            <add name="MyReceiver" type="TestHost.MyReceiver, MyAssembly" 
               idempotent="true"
               idempotencyCacheStrategy="file"
               idempotencyCacheExpirationSpan="00:20:00" />
            <add name="MyOtherReceiver" type="TestHost.MyOtherReceiver, MyAssembly"
                graph="http://acme.com/some-graph" />
        </Receivers>
    </SdShareReceiverConfigurationSection>
</configuration>
```

* __configSections__ The config section named *SdShareReceiverConfigurationSection* must be added in the configSections element
* __SdShareReceiverConfigurationSection__ All configuration for the push receiver is contained within this element.
* __SdShareReceiverConfigurationSection.port__ __Mandatory__ The port that the endpoint will be hosted on.
* __Receivers__ The list of all receivers, i.e. classes implementing IFragmentReceiver that are able to handle incoming fragments
* __add__ Each receiver is included via the add element
* __add.name__ __Mandatory__ A unique name for the receiver
* __add.type__ __Mandatory__ The assembly qualified name of the type implementing IFragmentReceiver
* __add.graph__ Is used as a filter, and if configured for a receiver, the receiver will only receive notice if the graph is included as a parameter in the url of the request. Receivers that are not configured with the graph attribute will receive all requests regardless.
* __add.idempotent__ Whether support for idempotency should be included for the receiver. Defaults to *false* if not included.
* __add.idempotencyCacheStrategy__ Is only used if add.idempotent == true. The two possible values are *file* and *memory*. Defaults to *memory* if not included.
* __add.idempotencyCacheExpirationSpan__ Specifies the duration that idempotency cache should retain each message. Defaults to one hour (*01:00:00*) if not included.

Hosting
-------
The code snippet below shows how you can utilise the functionality of *SdShare.Service.AspNetWebApi.dll* inside of your host application in order to spin up the push receiver.

The host will automatically listen to the url http://localhost:9001 (given that the port 9001 is configured in the xml config). A sample of a valid url would the be:

__http://localhost:9001/?resource=http://myresource&resource=http://myotherresource&graph=http://mygraph__

```C#
using Microsoft.Owin.Hosting;
using SdShare.Configuration;

var options = new StartOptions();
foreach (var address in EndpointConfiguration.Addresses)
{
	_options.Urls.Add(address);
}

using (WebApp.Start<SetUpConfiguration>(options))
{
	// Don't exit this using block until
	// the application is shut down. 
}
```