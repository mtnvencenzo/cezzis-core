namespace Cezzi.Applications;

using System;
using System.Text.RegularExpressions;

/// <summary>
/// 
/// </summary>
public static class AlphabeticalCharConverter
{
    /// <summary>Converts an interger to it's equivlent aphabet character. I.e. 1 = A, 2 = B.</summary>
    /// <param name="value">The integer to convert to its alpha character.  The supplied integer ust be between 1 and 26</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException">value - value must be in the range of 1-26</exception>
    public static char FromInteger(int value)
    {
        return value is < 1 or > 26
            ? throw new ArgumentOutOfRangeException(nameof(value), "value must be in the range of 1-26")
            : (char)(value + 64);
    }

    /// <summary>Converts a character to its numeric alphabet position. I.e. C = 3, D = 4</summary>
    /// <param name="value">The character to convert.  Must be uppercase and in the range of A-Z.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException">value - value must be in the range of A-Z uppercase.</exception>
    public static int ToInteger(char value)
    {
        var strVersion = value.ToString();

        return !Regex.IsMatch(strVersion, "^[A-Z]{1}$")
            ? throw new ArgumentOutOfRangeException(nameof(value), "value must be in the range of A-Z uppercase.")
            : value - 64;
    }
}
