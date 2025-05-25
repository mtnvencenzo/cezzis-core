namespace Cezzi.Caching.Core;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Caching.Core.InProcCacheProvideBase" />
public class DefaultInProcCacheProvider : InProcCacheProvideBase
{
    private readonly static InProcCacheData cacheData;

    /// <summary>
    /// Initializes the <see cref="DefaultInProcCacheProvider"/> class.
    /// </summary>
    static DefaultInProcCacheProvider()
    {
        cacheData = new InProcCacheData();
    }

    /// <summary>Gets the cache data.</summary>
    /// <returns></returns>
    protected override InProcCacheData GetCacheData() => cacheData;
}
