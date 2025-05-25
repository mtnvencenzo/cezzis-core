# Cezzi.Caching.Redis
Wrapper object around the [StackExchange.Redis](https://stackexchange.github.io/StackExchange.Redis/) Redis cache client.
<br/>

|  |  |  
|---|---|
| **Source Code** | [$/Cezzi/src/Cezzi.Caching.Redis](https://dev.azure.com/mtnvencenzo/Cezzi/_git/Cezzi?path=/Cezzi/src/Cezzi.Caching.Redis) | 
| **Nuget** | [![Cezzi.Caching.Redis package in Global feed in Azure Artifacts](https://feeds.dev.azure.com/mtnvencenzo/_apis/public/Packaging/Feeds/Global/Packages/0b236b42-b182-4b2a-8bdd-82123ea97551/Badge)](https://dev.azure.com/mtnvencenzo/Cezzi/_artifacts/feed/Global/NuGet/Cezzi.Caching.Redis?preferRelease=true) |

<br/>

# Using the Redis Connection

## Registering the RedisConnection in DI
***Note*** This example uses the appsettings.json and IOptions patterns for configuration settings.  It is also using Azure Key Vault for pulling sensitive redis configuration values.

***The appsettings.json config.***
``` json
{
  "CommonKeyVaultSettings": {
    "UseKeyVaultSecrets": "true",
    "KeyVaultKeyPrefix": "dev-",
    "KeyVaultBaseUri": "https://kv-cezzi-dev-01.vault.azure.net"
  },
  "RedisConfig": {
    "ConnectionString": "redis-cezzi-dev.redis.cache.windows.net:6380,ssl=true,abortConnect=false,syncTimeout=1000,asyncTimeout=1000,connectTimeout=1500,connectRetry=1,version=3.0",
    "Password": "abcdefg",
    "ReconnectMinFrequency": "30",
    "ReconnectErrorThreshold": "15",
    "RetryMaxAttempts": "1"
  }
}
```

***Application startup and bootstrapping.***
``` csharp
namespace Cezzi.Api
{
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;
    using Cezzi.Caching.Redis;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using System;

    /// <summary>
    /// 
    /// </summary>6
    public class Startup
    {
        private readonly IWebHostEnvironment webhostEnvironment;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public IServiceProvider ServiceProvider { get; set; }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CommonKeyVaultSettings>(this.Configuration.GetSection(CommonKeyVaultSettings.CustomSectionName));

            // Redis Caching
            services.Configure<RedisConfig>(this.Configuration.GetSection(RedisConfig.SectionName));
            services.UseStackExchangeRedisServices(configBuilder: this.RedisConfigBuilder);
        }

        private RedisConfig RedisConfigBuilder(IServiceProvider serviceProvider)
        {
            var commonKeyVaultSettings = this.ServiceProvider.GetRequiredService<IOptionsMonitor<CommonKeyVaultSettings>>().CurrentValue;
            var redisConfig = this.ServiceProvider.GetRequiredService<IOptionsMonitor<CezziRedisConfig>>().CurrentValue;

            if (commonKeyVaultSettings.UseKeyVaultSecrets)
            {
                var secretClient = new SecretClient(
                    vaultUri: new Uri(commonKeyVaultSettings.KeyVaultBaseUri),
                    credential: new DefaultAzureCredential(false));

                redisConfig.Password = this.GetKeyVaultSecret(secretClient, $"{commonKeyVaultSettings.KeyVaultKeyPrefix}cezzi-redis-services-password");
            }

            return redisConfig;
        }

        private string GetKeyVaultSecret(SecretClient secretClient, string key)
        {
            return secretClient
                .GetSecretAsync(key)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()
                .Value
                .Value;
        }
    }
}
```
***Using the RedisConnection***
``` csharp
namespace Cezzi.Services
{
    using Cezzi.Caching.Redis;
    using StackExchange.Redis;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RedisDuplicateRequestDetector
    {
        private readonly RedisConnection redisConnection;

        public RedisDuplicateRequestDetector(RedisConnection redisConnection)
        {
            this.redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
        }

        public async Task<DuplicateDetectionResult> Verify(string key, CancellationToken cancellationToken = default)
        {
            var set = await this.redisConnection.GetDatabase().StringSetAsync(
                key: $"myprefix{key}",
                value: true,
                expiry: TimeSpan.FromSeconds(60),
                when: When.NotExists).ConfigureAwait(false);

            if (!set)
            {
                return new DuplicateDetectionResult(true);
            }

            return new DuplicateDetectionResult();
        }
    }

    public class DuplicateDetectionResult
    {
        public DuplicateDetectionResult(bool isDuplicate = false)
        {
            this.IsDuplicate = isDuplicate;
        }

        public bool IsDuplicate { get; }
    }
}
```
