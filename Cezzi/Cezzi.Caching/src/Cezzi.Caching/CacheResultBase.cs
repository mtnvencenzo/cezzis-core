namespace Cezzi.Caching;

/// <summary>
/// 
/// </summary>
public abstract class CacheResultBase
{
    /// <summary>Gets or sets the result.</summary>
    /// <value>The result.</value>
    public virtual CacheResult Result { get; set; }

    /// <summary>Gets or sets the location.</summary>
    /// <value>The location.</value>
    public virtual CacheLocation Location { get; set; }

    /// <summary>Gets or sets the key.</summary>
    /// <value>The key.</value>
    public virtual CacheKey Key { get; set; }
}
