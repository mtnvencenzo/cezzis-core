namespace Cezzi.Applications.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 
/// </summary>
public static class ObjectExtensions
{
    /// <summary>Projects the specified projection.</summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="obj">The object.</param>
    /// <param name="projection">The projection.</param>
    /// <returns></returns>
    public static TResult Project<T, TResult>(this T obj, Func<T, TResult> projection) => obj == null || projection == null ? default : projection(obj);

    /// <summary>
    /// Recursively build up a list of custom types included in an object.  Allows
    /// runtime determination of all available types for XML serialization.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetSubTypes(object item)
    {
        if (item == null)
        {
            yield break;
        }

        var itemType = item.GetType();
        yield return itemType;
        foreach (
            var prop in
                itemType.GetProperties()
                    .Where(
                        p =>
                            p.DeclaringType != null && p.PropertyType.IsClass &&
                            !p.PropertyType.FullName.StartsWith("System.", StringComparison.CurrentCultureIgnoreCase))
            )
        {
            var propValue = prop.GetValue(item);
            if (propValue != null)
            {
                yield return propValue.GetType();
            }

            foreach (var t in GetSubTypes(propValue))
            {
                yield return t;
            }
        }
    }

    /// <summary>Determines whether the specified items is in.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item">The item.</param>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static bool IsIn<T>(this T item, IEnumerable<T> items) => items.Contains(item);

    /// <summary>Determines whether the specified items is in.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item">The item.</param>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static bool IsIn<T>(this T item, params T[] items) => items.Contains(item);

    /// <summary>Whens the null.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item">The item.</param>
    /// <param name="defaultItem">The default item.</param>
    /// <returns></returns>
    public static T WhenNull<T>(this T item, T defaultItem)
    {
        return (item == null)
            ? defaultItem
            : item;
    }
}
