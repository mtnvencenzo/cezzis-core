namespace Cezzi.Caching.Core;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Caching.Core.InProcCacheProvideBase" />
public class InjectableInProcCacheProvider : InProcCacheProvideBase
{
    private readonly InProcCacheData cacheData;

    /// <summary>
    /// Initializes the <see cref="DefaultInProcCacheProvider"/> class.
    /// </summary>
    public InjectableInProcCacheProvider()
    {
        this.cacheData = new InProcCacheData();
    }

    /// <summary>Gets the cache data.</summary>
    /// <returns></returns>
    protected override InProcCacheData GetCacheData() => this.cacheData;
}
