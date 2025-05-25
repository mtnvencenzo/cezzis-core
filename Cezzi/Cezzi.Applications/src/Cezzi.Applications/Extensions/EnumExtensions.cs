namespace Cezzi.Applications.Extensions;

using System;

/// <summary>
/// 
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Ases the enum.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="val">The value.</param>
    /// <returns></returns>
    public static T AsEnum<T>(this int val) where T : struct, IComparable, IFormattable, IConvertible
    {
        try
        {
            return (T)Enum.ToObject(typeof(T), val);
        }
        catch
        {
            return default;
        }
    }

    /// <summary>Asynchronous the enum.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inString">The information string.</param>
    /// <param name="ignoreCase">If set to <c>true</c> [ignore case].</param>
    /// <returns>The enumeration that was created.</returns>
    /// <exception cref="System.ArgumentException">T must be an enum type.</exception>
    public static T AsEnum<T>(this string inString, bool ignoreCase) where T : struct, IComparable, IFormattable, IConvertible
    {
        return AsEnum<T>(
            inString: inString,
            ignoreCase: ignoreCase,
            replacementChar: null);
    }

    /// <summary>
    /// Ases the enum.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inString">The in string.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <param name="replacementChar">The replacement character.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">T must be an enum type</exception>
    public static T AsEnum<T>(this string inString, bool ignoreCase, string replacementChar) where T : struct, IComparable, IFormattable, IConvertible
    {
        if (string.IsNullOrWhiteSpace(replacementChar))
        {
            replacementChar = "_";
        }

        return !typeof(T).IsEnum
            ? throw new ArgumentException("T must be an enum type")
            : ParseCommaSeparatedFlagedEnum<T>(
            inString: inString,
            ignoreCase: ignoreCase,
            itemProcessor: null,
            replacementChar: replacementChar);
    }

    /// <summary>Asynchronous the string.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumMember">The enum member.</param>
    /// <returns>The <see cref="string"/>.</returns>
    /// <exception cref="System.ArgumentException">T must be an enum type.</exception>
    public static string AsString<T>(this T enumMember) where T : struct, IComparable, IFormattable, IConvertible
    {
        return AsString(
            enumMember: enumMember,
            replacementChar: null);
    }

    /// <summary>Ases the string.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumMember">The enum member.</param>
    /// <param name="replacementChar">The replacement character to use when replacing underscore characters.  The default is a space.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">T must be an enum type</exception>
    public static string AsString<T>(this T enumMember, string replacementChar) where T : struct, IComparable, IFormattable, IConvertible
    {
        if (string.IsNullOrWhiteSpace(replacementChar))
        {
            replacementChar = " ";
        }

        return !typeof(T).IsEnum
            ? throw new ArgumentException("T must be an enum type")
            : enumMember.ToString().Replace("_", replacementChar);
    }

    /// <summary>Determines whether [is single flagged enumeration member] [the specified types].</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="types">The types.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    /// <exception cref="System.ArgumentException">EnumType must be an enumeration.</exception>
    public static bool IsSingleFlaggedEnumMember<T>(this T types) where T : struct, IComparable, IFormattable, IConvertible
    {
        var enumType = types.GetType();

        return !enumType.IsEnum
            ? throw new ArgumentException("enumType must be an enum")
            : !types
            .ToString()
            .Replace(" ", string.Empty)
            .Contains(',');
    }

    /// <summary>Parses the comma separated flaged enum.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inString">The in string.</param>
    /// <returns></returns>
    public static T ParseCommaSeparatedFlagedEnum<T>(string inString) where T : struct, IComparable, IFormattable, IConvertible
    {
        return ParseCommaSeparatedFlagedEnum<T>(
            inString: inString,
            ignoreCase: false);
    }

    /// <summary>Parses the comma separated flaged enum.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inString">The in string.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <returns></returns>
    public static T ParseCommaSeparatedFlagedEnum<T>(
        string inString,
        bool ignoreCase) where T : struct, IComparable, IFormattable, IConvertible
    {
        return ParseCommaSeparatedFlagedEnum<T>(
            inString: inString,
            ignoreCase: ignoreCase,
            itemProcessor: null);
    }

    /// <summary>Parses the comma separated flaged enum.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inString">The in string.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <param name="itemProcessor">The item processor.</param>
    /// <returns></returns>
    public static T ParseCommaSeparatedFlagedEnum<T>(string inString, bool ignoreCase, Func<string, string> itemProcessor) where T : struct, IComparable, IFormattable, IConvertible
    {
        return ParseCommaSeparatedFlagedEnum<T>(
            inString: inString,
            ignoreCase: ignoreCase,
            itemProcessor: itemProcessor,
            replacementChar: null);
    }

    /// <summary>Parses the comma separated flaged enum.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inString">The in string.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <param name="itemProcessor">The item processor.</param>
    /// <param name="replacementChar">The replacement character.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">types must be an enum type</exception>
    public static T ParseCommaSeparatedFlagedEnum<T>(
        string inString,
        bool ignoreCase,
        Func<string, string> itemProcessor,
        string replacementChar) where T : struct, IComparable, IFormattable, IConvertible
    {
        if (string.IsNullOrWhiteSpace(replacementChar))
        {
            replacementChar = "_";
        }

        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("types must be an enum type");
        }

        if (string.IsNullOrWhiteSpace(inString))
        {
            return default;
        }

        T enumTypes = default;

        foreach (var type in inString.Split([","], StringSplitOptions.RemoveEmptyEntries))
        {
            var processedType = (itemProcessor != null) ? itemProcessor(type) : type;

            if (processedType != null)
            {
                processedType = processedType.Replace(" ", replacementChar);
                processedType = processedType.Replace("-", replacementChar);
            }

            if (!string.IsNullOrWhiteSpace(processedType))
            {
                if (Enum.TryParse(processedType, ignoreCase, out T enumType))
                {
                    if (Convert.ToInt64(enumType) != Convert.ToInt64(default(T)))
                    {
                        if (Convert.ToInt64(enumTypes) == Convert.ToInt64(default(T)))
                        {
                            enumTypes = enumType;
                        }
                        else
                        {
                            var current = Convert.ToInt64(enumTypes);
                            var currentOr = Convert.ToInt64(enumType);
                            current |= currentOr;
                            enumTypes = (T)Enum.ToObject(typeof(T), current);
                        }
                    }
                }
            }
        }

        return enumTypes;
    }

    /// <summary>To the comma separated string.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="types">The types.</param>
    /// <returns>The <see cref="string"/>.</returns>
    /// <exception cref="System.ArgumentException">EnumType must be an enumeration.</exception>
    public static string ToCommaSeparatedString<T>(this T types) where T : struct, IComparable, IFormattable, IConvertible
    {
        var enumType = types.GetType();

        return !enumType.IsEnum ? throw new ArgumentException("enumType must be an enum") : types.ToString().Replace(" ", string.Empty);
    }

    /// <summary>Gets the type of the attribute of.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumVal">The enum value.</param>
    /// <returns></returns>
    public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);

        return (attributes.Length > 0)
            ? (T)attributes[0]
            : null;
    }
}
