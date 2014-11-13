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

Configuration
-------------
ssad

'
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
'

asdsa

Hosting
-------