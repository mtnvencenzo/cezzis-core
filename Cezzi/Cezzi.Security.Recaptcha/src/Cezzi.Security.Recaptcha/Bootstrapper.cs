namespace Cezzi.Security.Recaptcha;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class Bootstrapper
{
    /// <summary>Uses the drinks services.</summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IServiceCollection UseRecaptcha(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IRecaptchaSiteVerifyService, RecaptchaSiteVerifyService>();

        services.Configure<RecaptchaConfig>(configuration.GetSection(RecaptchaConfig.SectionName));

        services.AddHttpClient(RecaptchaSiteVerifyService.HttpClientName);

        return services;
    }
}
