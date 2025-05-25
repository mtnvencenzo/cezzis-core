namespace Cezzi.Data.Extensions;

using System;
using System.Data;

/// <summary>
/// 
/// </summary>
public static class IDataParameterCollectionExtensions
{
    /// <summary>Determines whether the specified parameters has parameter.</summary>
    /// <typeparam name="TParams">The type of the parameters.</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <param name="name">The name.</param>
    /// <returns><c>true</c> if the specified parameters has parameter; otherwise, <c>false</c>.</returns>
    public static bool HasParameter<TParams>(this TParams parameters, string name) where TParams : IDataParameterCollection
    {
        if (parameters == null)
        {
            return false;
        }

        foreach (var parameter in parameters)
        {
            if (((IDataParameter)parameter).ParameterName == name)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Determines whether the specified parameters has parameter.</summary>
    /// <typeparam name="TParams">The type of the parameters.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if the specified parameters has parameter; otherwise, <c>false</c>.</returns>
    public static bool HasParameter<TParams, TValue>(this TParams parameters, string name, TValue value) where TParams : IDataParameterCollection
    {
        if (parameters == null)
        {
            return false;
        }

        foreach (var parameter in parameters)
        {
            if (((IDataParameter)parameter).ParameterName == name)
            {
                if (IsNullValue(((IDataParameter)parameter).Value) && IsNullValue(value))
                {
                    return true;
                }

                if (IsNullValue(((IDataParameter)parameter).Value))
                {
                    return false;
                }

                if (IsNullValue(value))
                {
                    return false;
                }

                if (((IDataParameter)parameter).Value.Equals(value))
                {
                    return true;
                }

                if (((IDataParameter)parameter).Value.ToString().Equals(value.ToString()))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>Determines whether the specified parameters has parameter.</summary>
    /// <typeparam name="TParams">The type of the parameters.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if the specified parameters has parameter; otherwise, <c>false</c>.</returns>
    public static bool HasParameter<TParams, TValue>(this TParams parameters, string name, TValue value, DbType type) where TParams : IDataParameterCollection
    {
        if (parameters == null)
        {
            return false;
        }

        foreach (var parameter in parameters)
        {
            if (((IDataParameter)parameter).ParameterName == name)
            {
                if (((IDataParameter)parameter).DbType != type)
                {
                    return false;
                }

                if (IsNullValue(((IDataParameter)parameter).Value) && IsNullValue(value))
                {
                    return true;
                }

                if (IsNullValue(((IDataParameter)parameter).Value))
                {
                    return false;
                }

                if (IsNullValue(value))
                {
                    return false;
                }

                if (((IDataParameter)parameter).Value.Equals(value))
                {
                    return true;
                }

                if (((IDataParameter)parameter).Value.ToString().Equals(value.ToString()))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>Determines whether [has parameter of type] [the specified name].</summary>
    /// <typeparam name="TParams">The type of the parameters.</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <param name="name">The name.</param>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if [has parameter of type] [the specified name]; otherwise, <c>false</c>.</returns>
    public static bool HasParameterOfType<TParams>(this TParams parameters, string name, DbType type) where TParams : IDataParameterCollection
    {
        if (parameters == null)
        {
            return false;
        }

        foreach (var parameter in parameters)
        {
            if (((IDataParameter)parameter).ParameterName == name)
            {
                return ((IDataParameter)parameter).DbType == type;
            }
        }

        return false;
    }

    /// <summary>Determines whether [has parameter containing value] [the specified parameters].</summary>
    /// <typeparam name="TParams">The type of the parameters.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if [has parameter containing value] [the specified parameters]; otherwise, <c>false</c>.</returns>
    public static bool HasParameterContainingValue<TParams, TValue>(this TParams parameters, string name, TValue value, DbType type) where TParams : IDataParameterCollection
    {
        if (parameters == null)
        {
            return false;
        }

        foreach (var parameter in parameters)
        {
            if (((IDataParameter)parameter).ParameterName == name)
            {
                if (((IDataParameter)parameter).DbType != type)
                {
                    return false;
                }

                if (IsNullValue(((IDataParameter)parameter).Value) && IsNullValue(value))
                {
                    return true;
                }

                if (IsNullValue(((IDataParameter)parameter).Value))
                {
                    return false;
                }

                if (IsNullValue(value))
                {
                    return false;
                }

                if (((IDataParameter)parameter).Value.Equals(value))
                {
                    return true;
                }

                if (((IDataParameter)parameter).Value.ToString().Equals(value.ToString()))
                {
                    return true;
                }

                if (((IDataParameter)parameter).Value.ToString().Contains(value.ToString()))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>Determines whether [has parameter with value match] [the specified name].</summary>
    /// <typeparam name="TParams">The type of the parameters.</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <param name="name">The name.</param>
    /// <param name="valueMatcher">The value matcher.</param>
    /// <returns><c>true</c> if [has parameter with value match] [the specified name]; otherwise, <c>false</c>.</returns>
    public static bool HasParameterWithValueMatch<TParams>(this TParams parameters, string name, Predicate<object> valueMatcher) where TParams : IDataParameterCollection
    {
        if (parameters == null)
        {
            return false;
        }

        foreach (var parameter in parameters)
        {
            if (((IDataParameter)parameter).ParameterName == name)
            {
                if (valueMatcher(((IDataParameter)parameter).Value))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>Determines whether [has output parameter] [the specified name].</summary>
    /// <typeparam name="TParams">The type of the parameters.</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <param name="name">The name.</param>
    /// <returns><c>true</c> if [has output parameter] [the specified name]; otherwise, <c>false</c>.</returns>
    public static bool HasOutputParameter<TParams>(this TParams parameters, string name) where TParams : IDataParameterCollection
    {
        if (parameters == null)
        {
            return false;
        }

        foreach (var parameter in parameters)
        {
            if (((IDataParameter)parameter).ParameterName == name)
            {
                if (((IDataParameter)parameter).Direction == ParameterDirection.Output)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>Determines whether [has return value parameter] [the specified name].</summary>
    /// <typeparam name="TParams">The type of the parameters.</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <param name="name">The name.</param>
    /// <returns><c>true</c> if [has return value parameter] [the specified name]; otherwise, <c>false</c>.</returns>
    public static bool HasReturnValueParameter<TParams>(this TParams parameters, string name) where TParams : IDataParameterCollection
    {
        if (parameters == null)
        {
            return false;
        }

        foreach (var parameter in parameters)
        {
            if (((IDataParameter)parameter).ParameterName == name)
            {
                if (((IDataParameter)parameter).Direction == ParameterDirection.ReturnValue)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsNullValue(object v) => v == null || v == DBNull.Value;
}
