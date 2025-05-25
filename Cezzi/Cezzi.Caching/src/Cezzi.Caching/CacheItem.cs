namespace Cezzi.Caching;

using System;
using System.Threading;

/// <summary>
/// 
/// </summary>
public class CacheItem
{
    /// <summary>Gets or sets the hits.</summary>
    /// <value>The hits.</value>
    private int hits;

    /// <summary>Gets or sets the item.</summary>
    /// <value>The item.</value>
    public object Item { get; set; }

    /// <summary>Gets or sets the created.</summary>
    /// <value>The created.</value>
    public DateTime Created { get; set; }

    /// <summary>Gets or sets the expires.</summary>
    /// <value>The expires.</value>
    public DateTime Expires { get; set; }

    /// <summary>Gets a value indicating whether this instance is expired.</summary>
    /// <value>
    /// <c>true</c> if this instance is expired; otherwise, <c>false</c>.
    /// </value>
    public bool IsExpired => this.Expires <= DateTime.UtcNow;

    /// <summary>Increments the hits.</summary>
    public void IncrementHits() => Interlocked.Increment(ref this.hits);

    /// <summary>Gets the hits.</summary>
    /// <returns></returns>
    public int Hits => this.hits;
}
