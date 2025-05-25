namespace Cezzi.Applications.Extensions;

using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// 
/// </summary>
public static class TypeExtensions
{
    /// <summary>Determines whether the specified type is nullable.</summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static bool IsNullable(this Type type) => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    /// <summary>Gets the custom attributes.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static Collection<T> GetCustomAttributes<T>(this Type type) where T : class => type.GetCustomAttributes<T>(false);

    /// <summary>Gets the custom attributes.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type">The type.</param>
    /// <param name="inherit">if set to <c>true</c> [inherit].</param>
    /// <returns></returns>
    public static Collection<T> GetCustomAttributes<T>(this Type type, bool inherit) where T : class
    {
        var col = new Collection<T>();

        var attributes = type.GetCustomAttributes<T>(inherit);

        if (attributes != null)
        {
            foreach (var attr in attributes)
            {
                col.Add(attr);
            }
        }

        return col;
    }

    /// <summary>Gets the default value.</summary>
    /// <param name="type">The type. <see cref="Type"/></param>
    /// <returns>The <see cref="object"/>.</returns>
    public static object GetDefaultValue(this Type type)
    {
        // We want an Func<object> which returns the default.
        // Create that expression here.
        var e = Expression.Lambda<Func<object>>(Expression.Convert(Expression.Default(type), typeof(object)));
        return e.Compile()();
    }
}
