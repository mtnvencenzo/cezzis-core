namespace Cezzi.Applications.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 
/// </summary>
public static class IListExtensions
{
    /// <summary>Ares all null.</summary>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static bool AreAllNull(params object[] items)
    {
        Guard.NotNull(items, nameof(items));
        Guard.NotEmpty(items, nameof(items));

        foreach (var item in items)
        {
            if (item != null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>Ares all null.</summary>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static bool AreAllNotNull(params object[] items)
    {
        Guard.NotNull(items, nameof(items));
        Guard.NotEmpty(items, nameof(items));

        foreach (var item in items)
        {
            if (item == null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>Nots the null count.</summary>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static int NotNullCount(params object[] items)
    {
        return items == null
            ? 0
            : items
            .Where(i => i != null)
            .Count();
    }

    /// <summary>Nots the null count.</summary>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static int NullCount(params object[] items)
    {
        return items == null
            ? 0
            : items
            .Where(i => i == null)
            .Count();
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

    /// <summary>Fors the each.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    public static IList<T> ForEach<T>(this IList<T> items, Action<T> action)
    {
        IList<T> newItems = [];

        foreach (var item in items)
        {
            action(item);
            newItems.Add(item);
        }

        return newItems;
    }

    /// <summary>Fors the each.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">The items.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        IList<T> newItems = [];

        foreach (var item in items)
        {
            action(item);
            newItems.Add(item);
        }

        return newItems;
    }

    /// <summary>Existses the specified exists.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">The items.</param>
    /// <param name="exists">The exists.</param>
    /// <returns></returns>
    public static bool Exists<T>(this IList<T> items, Predicate<T> exists)
    {
        foreach (var item in items)
        {
            if (exists(item))
            {
                return true;
            }
        }

        return false;
    }
}
