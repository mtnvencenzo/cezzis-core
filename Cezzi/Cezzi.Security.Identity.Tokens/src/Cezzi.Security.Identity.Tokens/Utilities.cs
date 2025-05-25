namespace Cezzi.Security.Identity.Tokens;

using System;

/// <summary>
/// 
/// </summary>
internal static class Utilities
{
    /// <summary>Ases the enum.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inString">The in string.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <returns></returns>
    internal static T AsEnum<T>(
        string inString,
        bool ignoreCase) where T : struct, IComparable, IFormattable, IConvertible
    {
        return !typeof(T).IsEnum
            ? throw new ArgumentException("T must be an enum type")
            : ParseCommaSeparatedFlagedEnum<T>(
            inString: inString,
            ignoreCase: ignoreCase,
            itemProcessor: null,
            replacementChar: null);
    }

    /// <summary>Parses the comma separated flaged enum.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inString">The in string.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <param name="itemProcessor">The item processor.</param>
    /// <param name="replacementChar">The replacement character.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">types must be an enum type</exception>
    internal static T ParseCommaSeparatedFlagedEnum<T>(
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
            var processedType = itemProcessor != null ? itemProcessor(type) : type;

            if (processedType != null)
            {
                processedType = processedType.Replace(" ", replacementChar);
                processedType = processedType.Replace("-", replacementChar);
            }

            if (!string.IsNullOrWhiteSpace(processedType))
            {
                if (Enum.TryParse<T>(processedType, ignoreCase, out var enumType))
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
}
