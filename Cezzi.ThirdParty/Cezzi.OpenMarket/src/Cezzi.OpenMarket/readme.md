# Cezzi.OpenMarket
Base library and wrapper for sending text message through [Open Market's Apis](https://www.openmarket.com/docs/Content/apis/v4http/send-xml.htm).  

> The main functionality from this library comes from the `OpenMarketSmsClient` which is just a wrapper around sending an http request to the OpenMarket send sms request.

<br/>


#### Register the OpenMarket Service Client with DI
``` csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();

        services.RegisterServiceClient<IOpenMarketSmsClient, OpenMarketSmsClient>((sp, client) =>
        {
            var config = sp.GetRequiredService<IOptionsMonitor<OpenMarketSmsConfiguration>>();
            client.BaseAddress = new Uri(config.CurrentValue.Host);
        });
    }
}
```
<br/>
