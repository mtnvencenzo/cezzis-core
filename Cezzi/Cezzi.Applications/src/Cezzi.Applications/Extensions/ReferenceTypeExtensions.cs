namespace Cezzi.Applications.Extensions;

using System;

/// <summary>
/// 
/// </summary>
public static class ReferenceTypeExtensions
{
    /// <summary>Nulls if.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns></returns>
    public static T NullIf<T>(this T value, Func<T, bool> predicate) where T : class
    {
        Guard.NotNull(predicate, nameof(predicate));

        return predicate(value) == false ? value : null;
    }
}
