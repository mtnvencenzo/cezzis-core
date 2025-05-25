namespace Cezzi.Applications;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// 
/// </summary>
public static class Guard
{
    /// <summary>Throws if the object provider is null.</summary>
    /// <param name="obj">The object.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static void NotNull(object obj, string paramName = null) => NotNull(obj, () => new ArgumentNullException(paramName));

    /// <summary>Throws the resolved exception if the object provider is null.</summary>
    /// <param name="obj"></param>
    /// <param name="exceptionResolver"></param>
    public static void NotNull(object obj, Func<Exception> exceptionResolver)
    {
        if (obj == null)
        {
            throw exceptionResolver();
        }
    }

    /// <summary>Throws if the string provider is null or whitespace.</summary>
    /// <param name="str">The string.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static void NotNullOrWhiteSpace(string str, string paramName = null) => NotNullOrWhiteSpace(str, () => new ArgumentNullException(paramName: paramName, message: $"String '{str}' is null or whitespace"));

    /// <summary>Throws the resolved exception if the string provider is null or whitespace.</summary>
    /// <param name="str"></param>
    /// <param name="exceptionResolver"></param>
    public static void NotNullOrWhiteSpace(string str, Func<Exception> exceptionResolver)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            throw exceptionResolver();
        }
    }

    /// <summary>Throws if the member provider is not the expected enum member.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <param name="member">The member.</param>
    /// <param name="paramName">Name of the parameter.</param>
    public static void NotEnumMember<T>(T value, T member, string paramName = null) where T : Enum => NotEnumMember<T>(value, member, () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {member} is not an allowable usage value for {value.GetType().Name}"));

    /// <summary>Throws if the member provider is not the expected enum member.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <param name="member">The member.</param>
    /// <param name="exceptionResolver">The exception to throw</param>
    public static void NotEnumMember<T>(T value, T member, Func<Exception> exceptionResolver) where T : Enum
    {
        if (value?.ToString() == member?.ToString())
        {
            throw exceptionResolver();
        }
    }

    /// <summary>Throws if the list contains more than one item.  A [null] list does not throw.</summary>
    /// <param name="list">The list.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void OneOrLess(IList list, string paramName = null) => OneOrLess(list, () => new ArgumentOutOfRangeException(paramName: paramName, message: $"List({list.Count}) contained more than 1 item."));

    /// <summary>Throws the resolved exception if the list contains more than one item.  A [null] list does not throw.</summary>
    /// <param name="list">The list.</param>
    /// <param name="exceptionResolver">The exception to throw</param>
    public static void OneOrLess(IList list, Func<Exception> exceptionResolver)
    {
        if (list == null)
        {
            return;
        }

        if (list.Count > 1)
        {
            throw exceptionResolver();
        }
    }

    /// <summary>Throws if the list is empty or [null]</summary>
    /// <param name="list">The list.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentException">
    /// </exception>
    public static void NotEmpty(IList list, string paramName = null) => NotEmpty(list, () => throw new ArgumentException(paramName: paramName, message: $"Excepted not empty but received {list?.Count.ToString() ?? "<null>"}."));

    /// <summary>Throws if the list is empty or [null]</summary>
    /// <param name="list">The list.</param>
    /// <param name="exceptionResolver">The exception to throw</param>
    public static void NotEmpty(IList list, Func<Exception> exceptionResolver)
    {
        if (list == null || list.Count == 0)
        {
            throw exceptionResolver();
        }
    }

    /// <summary>Throws if value is the default value of the type.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentException"></exception>
    public static void NotDefault<T>(T value, string paramName = null) => NotDefault<T>(value, () => new ArgumentException(paramName: paramName, message: $"{paramName} cannot be the default value for type {nameof(T)}"));

    /// <summary>Throws if value is the default value of the type.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <param name="exceptionResolver">The exception to throw</param>
    public static void NotDefault<T>(T value, Func<Exception> exceptionResolver)
    {
        T def = default;

        if (EqualityComparer<T>.Default.Equals(value, def))
        {
            throw exceptionResolver();
        }
    }

    /// <summary>Throws if not of a certain type.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentException"></exception>
    public static void OfType<T>(object value, string paramName = null) => OfType<T>(value, () => new ArgumentException(paramName: paramName, message: $"value is not of type {typeof(T).Name}"));

    /// <summary>Throws if not of a certain type.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <param name="exceptionResolver">The exception to throw</param>
    public static void OfType<T>(object value, Func<Exception> exceptionResolver)
    {
        Guard.NotNull(value, nameof(value));

        if (value.GetType() != typeof(T))
        {
            throw exceptionResolver();
        }
    }

    /// <summary>Throw if not positive</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Positive(short value, string paramName = null) => Positive(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not positive"));

    /// <summary>Throw if not positive</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Positive(int value, string paramName = null) => Positive(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not positive"));

    /// <summary>Throw if not positive</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Positive(long value, string paramName = null) => Positive(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not positive"));

    /// <summary>Throw if not positive</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Positive(float value, string paramName = null) => Positive(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not positive"));

    /// <summary>Throw if not positive</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Positive(double value, string paramName = null) => Positive(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not positive"));

    /// <summary>Throw if not positive</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Positive(decimal value, string paramName = null) => Positive(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not positive"));

    /// <summary>Throw if not negative</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Negative(short value, string paramName = null) => Negative(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not negative"));

    /// <summary>Throw if not negative</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Negative(int value, string paramName = null) => Negative(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not negative"));

    /// <summary>Throw if not negative</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Negative(long value, string paramName = null) => Negative(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not negative"));

    /// <summary>Throw if not negative</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Negative(float value, string paramName = null) => Negative(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not negative"));

    /// <summary>Throw if not negative</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Negative(double value, string paramName = null) => Negative(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not negative"));

    /// <summary>Throw if not negative</summary>
    /// <param name="value">The value.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void Negative(decimal value, string paramName = null) => Negative(value.ToString(), () => new ArgumentOutOfRangeException(paramName: paramName, actualValue: value, message: $"Value {value} is not negative"));

    private static void Positive(string value, Func<Exception> exceptionResolver)
    {
        if (double.Parse(value) <= 0)
        {
            throw exceptionResolver();
        }
    }

    private static void Negative(string value, Func<Exception> exceptionResolver)
    {
        if (double.Parse(value) >= 0)
        {
            throw exceptionResolver();
        }
    }
}
