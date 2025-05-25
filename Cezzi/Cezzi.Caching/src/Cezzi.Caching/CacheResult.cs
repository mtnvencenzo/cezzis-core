namespace Cezzi.Caching;

using System;

/// <summary>
/// 
/// </summary>
[Flags]
public enum CacheResult
{
    /// <summary>The none</summary>
    None = 0,

    /// <summary>The miss</summary>
    Miss = 1,

    /// <summary>The hit</summary>
    Hit = 2,

    /// <summary>The set</summary>
    Put = 4,

    /// <summary>The deleted</summary>
    Deleted = 8,

    /// <summary>The cleared</summary>
    Cleared = 16,

    /// <summary>The added</summary>
    Added = 32,

    /// <summary>The updated</summary>
    Updated = 64,

    /// <summary>The unavailable</summary>
    Unavailable = 128,

    /// <summary>The expired</summary>
    Expired = 256
}
