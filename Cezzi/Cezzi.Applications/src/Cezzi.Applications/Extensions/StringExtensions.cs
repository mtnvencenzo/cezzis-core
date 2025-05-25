namespace Cezzi.Applications.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// 
/// </summary>
public static class StringExtensions
{
    /// <summary>Nulls if white space.</summary>
    /// <param name="inString">The in string.</param>
    /// <returns></returns>
    public static string NullIfNullOrWhiteSpace(this string inString) => inString.NullIf(string.IsNullOrWhiteSpace);

    /// <summary>Alls the are null or white space.</summary>
    /// <param name="strings">The strings.</param>
    /// <returns></returns>
    public static bool AllAreNullOrWhiteSpace(params string[] strings)
    {
        Guard.NotNull(strings, nameof(strings));

        return strings.All(string.IsNullOrWhiteSpace);
    }

    /// <summary>Alls the are not null or white space.</summary>
    /// <param name="strings">The strings.</param>
    /// <returns></returns>
    public static bool AllAreNotNullOrWhiteSpace(params string[] strings)
    {
        Guard.NotNull(strings, nameof(strings));

        return strings.All(s => !string.IsNullOrWhiteSpace(s));
    }

    /// <summary>Truncates the specified length.</summary>
    /// <param name="inString">The in string.</param>
    /// <param name="length">The length.</param>
    /// <returns></returns>
    public static string Truncate(this string inString, int length) => string.IsNullOrEmpty(inString) ? inString : inString.Length > length ? inString[..length] : inString;

    /// <summary>Trims the safe.</summary>
    /// <param name="inString">The in string.</param>
    /// <returns></returns>
    public static string TrimSafe(this string inString) => inString?.Trim();

    /// <summary>Removes all white space.</summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static string RemoveAllWhiteSpace(this string input)
    {
        return input == null
            ? null
            : new string([.. input.ToCharArray().Where(c => !char.IsWhiteSpace(c))]);
    }

    /// <summary>Removes all non numbers.</summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static string RemoveAllNonNumbers(this string input)
    {
        return input == null
            ? null
            : new string([.. input.ToCharArray().Where(char.IsNumber)]);
    }

    /// <summary>Determines whether [is numbers only].</summary>
    /// <param name="input">The input.</param>
    /// <returns><c>true</c> if [is numbers only] [the specified input]; otherwise, <c>false</c>.</returns>
    public static bool IsNumbersOnly(this string input)
    {
        Guard.NotNull(input, nameof(input));

        return Regex.IsMatch(input, "^[0-9]+$");
    }

    /// <summary>Ins the length.</summary>
    /// <param name="inString">The in string.</param>
    /// <param name="minLength">The minimum length.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    public static bool InLength(this string inString, int minLength, int maxLength) => InLength(inString, minLength, maxLength, false);

    /// <summary>Ins the length.</summary>
    /// <param name="inString">The in string.</param>
    /// <param name="minLength">The minimum length.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <param name="allowNull">If set to <c>true</c> [allow null].</param>
    /// <returns>The <see cref="bool"/>.</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">MinLength;Cannot be less than zero(0)
    ///     or
    ///     maxLength;Cannot be less than minLength.</exception>
    public static bool InLength(this string inString, int minLength, int maxLength, bool allowNull)
    {
        if (minLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minLength), minLength, "Cannot be less than zero(0)");
        }

        return maxLength < minLength
            ? throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Cannot be less than minLength")
            : (inString == null && allowNull && minLength == 0)
|| (inString != null && inString.Length >= minLength && inString.Length <= maxLength);
    }

    /// <summary>Ignores the case contains.</summary>
    /// <param name="compare">The compare.</param>
    /// <param name="compareTo">The compare automatic.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    public static bool IgnoreCaseContains(this string compare, string compareTo) => string.IsNullOrEmpty(compareTo) || (compare != null && compare.ToLower().Contains(compareTo.ToLower()));

    /// <summary>Determines whether the specified pattern is match.</summary>
    /// <param name="inString">The in string.</param>
    /// <param name="pattern">The pattern.</param>
    /// <returns></returns>
    public static bool IsMatch(this string inString, string pattern)
    {
        var reg = new Regex(pattern);
        return reg.IsMatch(inString);
    }

    /// <summary>Lefts the specified length.</summary>
    /// <param name="item">The item.</param>
    /// <param name="length">The length.</param>
    /// <returns></returns>
    public static string Left(this string item, int length) => item == null ? string.Empty : item[..Math.Min(item.Length, length)];

    /// <summary>Rights the specified length.</summary>
    /// <param name="item">The item.</param>
    /// <param name="length">The length.</param>
    /// <returns></returns>
    public static string Right(this string item, int length)
    {
        if (item == null)
        {
            return string.Empty;
        }

        var count = Math.Min(item.Length, length);

        return item.Substring(item.Length - count, count);
    }

    /// <summary>
    /// Determines whether the specified values is in.
    /// </summary>
    /// <param name="compare">The compare.</param>
    /// <param name="values">The values.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <returns>
    ///   <c>true</c> if the specified values is in; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsIn(this string compare, IEnumerable<string> values, bool ignoreCase)
    {
        if (compare == null || values == null)
        {
            return false;
        }

        foreach (var member in values)
        {
            if (string.Compare(compare, member, ignoreCase) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified item is number.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    public static bool IsNumeric(this string item) => item != null && item.Trim().IsMatch("^[0-9]+$");

    /// <summary>
    /// Trims the and truncate.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="length">The length.</param>
    /// <returns></returns>
    public static string TrimAndTruncate(this string item, int length) => item?.Trim().Truncate(length);

    /// <summary>
    /// Determines whether the specified item is a valid date.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    public static bool IsValidDate(this string item) => item != null && DateTime.TryParse(item, out var _);

    /// <summary>Subs the string safe.</summary>
    /// <param name="instr">The instr.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="length">The length.</param>
    /// <returns></returns>
    public static string SubstringSafe(this string instr, int startIndex = 0, int? length = null)
    {
        if (instr == null)
        {
            return null;
        }

        if (instr == string.Empty)
        {
            return string.Empty;
        }

        if (length <= 0)
        {
            return string.Empty;
        }

        var start = startIndex < 0
            ? 0
            : startIndex;

        if (start >= instr.Length)
        {
            return string.Empty;
        }

        if (length == null)
        {
            return instr[start..];
        }

        var useLength = length.Value > (instr.Length - start)
            ? instr.Length - start
            : length.Value;

        return instr.Substring(start, useLength);
    }

    /// <summary>Removes the invalid XML chars.</summary>
    /// <param name="instr">The instr.</param>
    /// <returns></returns>
    public static string RemoveInvalidXmlChars(this string instr) =>
        instr?
            .Replace("\"", string.Empty)
            .Replace("'", string.Empty)
            .Replace("<", string.Empty)
            .Replace(">", string.Empty)
            .Replace("&", string.Empty);

    /// <summary>Replaces the word special characters.</summary>
    /// <param name="instr">The instr.</param>
    /// <returns></returns>
    public static string ReplaceWordSpecialCharacters(this string instr) =>
        instr?
            .Replace('\u2013', '-')     // en dash
            .Replace('\u2014', '-')     // em dash
            .Replace('\u2015', '-')     // horizontal bar
            .Replace('\u2017', '_')     // double low line
            .Replace('\u2018', '\'')    // left single quotation mark
            .Replace('\u2019', '\'')    // right single quotation mark
            .Replace('\u201a', ',')     // single low-9 quotation mark
            .Replace('\u201b', '\'')    // single high-reversed-9 quotation mark
            .Replace('\u201c', '\"')    // left double quotation mark
            .Replace('\u201d', '\"')    // right double quotation mark
            .Replace('\u201e', '\"')    // double low-9 quotation mark
            .Replace("\u2026", "...")   // horizontal ellipsis
            .Replace('\u2032', '\'')    // prime
            .Replace('\u2033', '\"');   // double prime

    /// <summary>Replaces the tabs.</summary>
    /// <param name="instr">The instr.</param>
    /// <param name="replacement">The replacement.</param>
    /// <returns></returns>
    public static string ReplaceTabs(this string instr, string replacement = " ")
    {
        if (instr == null)
        {
            return null;
        }

        var value = instr?.Replace("\t", replacement);

        while (value.Contains("  "))
        {
            value = value.Replace("  ", " ");
        }

        return value;
    }

    /// <summary>
    /// Currently formats a phone number to a format of [800-]555-5555.  Replaces
    /// removes parenthesis and replaces dots with hyphens
    /// </summary>
    /// <param name="instr">The instr.</param>
    /// <param name="formatStyle">The format style.</param>
    /// <returns></returns>
    /// <example>800-555-5555</example>
    public static string FormatPhoneNumber(this string instr, PhoneFormatStyle formatStyle = PhoneFormatStyle.Standard)
    {
        return instr == null
            ? instr
            : formatStyle == PhoneFormatStyle.None
            ? instr
            : formatStyle == PhoneFormatStyle.Standard
            ? instr
                .Replace("(", "")
                .Replace(")", "-")
                .Replace(" ", "")
                .Replace(".", "-")
            : instr;
    }

    /// <summary>Prefixes the specified prefix.</summary>
    /// <param name="instring">The instring.</param>
    /// <param name="prefix">The prefix.</param>
    /// <returns></returns>
    public static string Prefix(this string instring, string prefix) => prefix + instring;

    /// <summary>This allows for a case insensitive search for a string in a string.</summary>
    /// <param name="inString">The source string.</param>
    /// <param name="value">The string to search for.</param>
    /// <param name="comparisonType">The comparison type.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    public static bool Contains(this string inString, string value, StringComparison comparisonType) => inString.Contains(value, comparisonType);

    /// <summary>To the int enumerable.</summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static IEnumerable<int> ToIntEnumerable(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            yield break;
        }

        var items = input.Split([","], StringSplitOptions.RemoveEmptyEntries);

        if (items != null)
        {
            foreach (var item in items)
            {
                if (int.TryParse(item, out var iItem))
                {
                    yield return iItem;
                }
            }
        }

        yield break;
    }

    /// <summary>Splits the specified separater.</summary>
    /// <param name="inString">The in string.</param>
    /// <param name="separater">The separater.</param>
    /// <param name="options">The options.</param>
    /// <returns></returns>
    public static string[] Split(this string inString, string separater, StringSplitOptions options) => inString.Split([separater], options);

    /// <summary>Whens the null or white space.</summary>
    /// <param name="item">The item.</param>
    /// <param name="defaultString">The default string.</param>
    /// <returns></returns>
    public static string WhenNullOrWhiteSpace(this string item, string defaultString)
    {
        return string.IsNullOrWhiteSpace(item)
            ? defaultString
            : item;
    }
}
