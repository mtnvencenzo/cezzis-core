namespace Cezzi.Applications.Text;

using Cezzi.Applications.Text.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

/// <summary>
/// 
/// </summary>
public static class RandomGen
{
    private readonly static Random firstNameRandomizer = new(Guid.NewGuid().GetHashCode());
    private readonly static Random lastNameRandomizer = new(Guid.NewGuid().GetHashCode());
    private readonly static Random streetRandomizer = new(Guid.NewGuid().GetHashCode());
    private readonly static Random cityRandomizer = new(Guid.NewGuid().GetHashCode());
    private readonly static Random stateRandomizer = new(Guid.NewGuid().GetHashCode());
    private readonly static Random addrRandomizer = new(Guid.NewGuid().GetHashCode());
    private readonly static Random usPostalRandomizer = new(Guid.NewGuid().GetHashCode());
    private readonly static Random usPostalLast4Randomizer = new(Guid.NewGuid().GetHashCode());
    private readonly static Random chooserRandomizer = new(Guid.NewGuid().GetHashCode());

    // Define default min and max password lengths.
    private const int DEFAULT_MIN_LENGTH = 8;
    private const int DEFAULT_MAX_LENGTH = 10;

    // Define supported password characters divided into groups.
    // You can add (or remove) characters to (from) these groups.
    /// <summary>The chars lcase</summary>
    public const string CHARS_LCASE = "abcdefghijklmnopqrstuvwxyz";
    /// <summary>The chars ucase</summary>
    public const string CHARS_UCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    /// <summary>The chars numeric</summary>
    public const string CHARS_NUMERIC = "0123456789";
    /// <summary>The chars symbols</summary>
    public const string CHARS_SYMBOLS = "!@#$%^&*()_-?<>[]{}|";

    /// <summary>Unfriendly characters that cause ambuigity in password
    /// </summary>
    public const string UNFRIENDLY_PASSWD_CHARS = "IOUV01ilouv[]{}|<>_-()*&^%";

    /// <summary>Generates this instance.</summary>
    /// <returns></returns>
    public static string Generate() => Generate(null);

    /// <summary>Generates the specified exclude.</summary>
    /// <param name="exclude">The exclude.</param>
    /// <returns></returns>
    public static string Generate(char[] exclude) => Generate(DEFAULT_MIN_LENGTH, DEFAULT_MAX_LENGTH, exclude);

    /// <summary>Generates the specified minimum length.</summary>
    /// <param name="minLength">The minimum length.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <returns></returns>
    public static string Generate(int minLength, int maxLength) => Generate(minLength, maxLength, CHARS_LCASE, CHARS_UCASE, CHARS_NUMERIC, CHARS_SYMBOLS);

    /// <summary>Generates the specified minimum length.</summary>
    /// <param name="minLength">The minimum length.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <param name="exclude">The exclude.</param>
    /// <returns></returns>
    public static string Generate(int minLength, int maxLength, char[] exclude)
    {
        return Generate(
            minLength,
            maxLength,
            string.Join(string.Empty, CHARS_LCASE.Except(exclude).Select(c => c.ToString())),
            string.Join(string.Empty, CHARS_UCASE.Except(exclude).Select(c => c.ToString())),
            string.Join(string.Empty, CHARS_NUMERIC.Except(exclude).Select(c => c.ToString())),
            string.Join(string.Empty, CHARS_SYMBOLS.Except(exclude).Select(c => c.ToString())));
    }

    /// <summary>Generates the specified minimum length.</summary>
    /// <param name="minLength">The minimum length.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <param name="include">The include.</param>
    /// <returns></returns>
    public static string Generate(int minLength, int maxLength, params string[] include)
    {
        // Make sure that input parameters are valid.
        if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
        {
            return null;
        }

        // Create a local array containing supported password characters
        // grouped by types. You can remove character groups from this
        // array, but doing so will weaken the password strength.
        var chars = new List<char[]>();
        foreach (var inc in include)
        {
            chars.Add([.. inc]);
        }

        var charGroups = chars.ToArray();

        // Use this array to track the number of unused characters in each
        // character group.
        var charsLeftInGroup = new int[charGroups.Length];

        // Initially, all characters in each group are not used.
        for (var i = 0; i < charsLeftInGroup.Length; i++)
        {
            charsLeftInGroup[i] = charGroups[i].Length;
        }

        // Use this array to track (iterate through) unused character groups.
        var leftGroupsOrder = new int[charGroups.Length];

        // Initially, all character groups are not used.
        for (var i = 0; i < leftGroupsOrder.Length; i++)
        {
            leftGroupsOrder[i] = i;
        }

        // Because we cannot use the default randomizer, which is based on the
        // current time (it will produce the same "random" number within a
        // second), we will use a random number generator to seed the
        // randomizer.

        // Use a 4-byte array to fill it with random bytes and convert it then
        // to an integer value.
        var randomBytes = new byte[4];

        // Generate 4 random bytes.
        var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomBytes);

        // Convert 4 bytes into a 32-bit integer value.
        var seed = ((randomBytes[0] & 0x7f) << 24) |
                    (randomBytes[1] << 16) |
                    (randomBytes[2] << 8) |
                    randomBytes[3];

        // Now, this is real randomization.
        var random = new Random(seed);

        // This array will hold password characters.
        var password = minLength < maxLength ? (new char[random.Next(minLength, maxLength + 1)]) : (new char[minLength]);

        // Allocate appropriate memory for the password.

        // Index of the next character to be added to password.
        int nextCharIdx;

        // Index of the next character group to be processed.
        int nextGroupIdx;

        // Index which will be used to track not processed character groups.
        int nextLeftGroupsOrderIdx;

        // Index of the last non-processed character in a group.
        int lastCharIdx;

        // Index of the last non-processed group.
        var lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

        // Generate password characters one at a time.
        for (var i = 0; i < password.Length; i++)
        {
            // If only one character group remained unprocessed, process it;
            // otherwise, pick a random character group from the unprocessed
            // group list. To allow a special character to appear in the
            // first position, increment the second parameter of the Next
            // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
            nextLeftGroupsOrderIdx = lastLeftGroupsOrderIdx == 0 ? 0 : random.Next(0, lastLeftGroupsOrderIdx);

            // Get the actual index of the character group, from which we will
            // pick the next character.
            nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

            // Get the index of the last unprocessed characters in this group.
            lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

            // If only one unprocessed character is left, pick it; otherwise,
            // get a random character from the unused character list.
            nextCharIdx = lastCharIdx == 0 ? 0 : random.Next(0, lastCharIdx + 1);

            // Add this character to the password.
            password[i] = charGroups[nextGroupIdx][nextCharIdx];

            // If we processed the last character in this group, start over.
            if (lastCharIdx == 0)
            {
                charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
            }

            // There are more unprocessed characters left.
            else
            {
                // Swap processed character with the last unprocessed character
                // so that we don't pick it until we process all characters in
                // this group.
                if (lastCharIdx != nextCharIdx)
                {
#pragma warning disable IDE0180 // Use tuple to swap values
                    var temp = charGroups[nextGroupIdx][lastCharIdx];
                    charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                    charGroups[nextGroupIdx][nextCharIdx] = temp;
#pragma warning restore IDE0180 // Use tuple to swap values
                }

                // Decrement the number of unprocessed characters in
                // this group.
                charsLeftInGroup[nextGroupIdx]--;
            }

            // If we processed the last group, start all over.
            if (lastLeftGroupsOrderIdx == 0)
            {
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            }

            // There are more unprocessed groups left.
            else
            {
                // Swap processed group with the last unprocessed group
                // so that we don't pick it until we process all groups.
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
#pragma warning disable IDE0180 // Use tuple to swap values
                    var temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                    leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                    leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
#pragma warning restore IDE0180 // Use tuple to swap values
                }
                // Decrement the number of unprocessed groups.
                lastLeftGroupsOrderIdx--;
            }
        }

        // Convert password characters into a string and return the result.
        return new string(password);
    }

    /// <summary>Firsts the name.</summary>
    /// <returns></returns>
    public static string FirstName()
    {
        var max = Sources.FirstNames.Count;

        return Sources.FirstNames[firstNameRandomizer.Next(minValue: 0, maxValue: max)];
    }

    /// <summary>Lasts the name.</summary>
    /// <returns></returns>
    public static string LastName()
    {
        var max = Sources.LastNames.Count;

        return Sources.LastNames[lastNameRandomizer.Next(minValue: 0, maxValue: max)];
    }

    /// <summary>Fulls the name.</summary>
    /// <param name="includeMiddleInitial">if set to <c>true</c> [include middle initial].</param>
    /// <returns></returns>
    public static string FullName(bool includeMiddleInitial = false) => includeMiddleInitial ? $"{FirstName()} {FirstName()[..1]} {LastName()}" : $"{FirstName()} {LastName()}";

    /// <summary>Streets the name.</summary>
    /// <returns></returns>
    public static string StreetName()
    {
        var max = Sources.StreetNames.Count;

        return Sources.StreetNames[streetRandomizer.Next(minValue: 0, maxValue: max)];
    }

    /// <summary>Addresses the name of the and street.</summary>
    /// <returns></returns>
    public static string AddressAndStreetName()
    {
        var max = Sources.StreetNames.Count;

        var addr = addrRandomizer.Next(minValue: 1, maxValue: 99999);
        var streetName = Sources.StreetNames[streetRandomizer.Next(minValue: 0, maxValue: max)];

        return $"{addr} {streetName}";
    }

    /// <summary>States the abbr.</summary>
    /// <returns></returns>
    public static string StateAbbr()
    {
        var max = Sources.StatesAndCities.Count;

        return Sources.StatesAndCities.Keys.ToList()[stateRandomizer.Next(minValue: 0, maxValue: max)];
    }

    /// <summary>Streets the name.</summary>
    /// <returns></returns>
    public static string CityName(string stateAbbreviation = null)
    {
        var doesAbbrExist = !string.IsNullOrWhiteSpace(stateAbbreviation) && Sources.StatesAndCities.ContainsKey(stateAbbreviation);

        var max = doesAbbrExist
            ? Sources.StatesAndCities[stateAbbreviation].Count
            : Sources.StatesAndCities.Sum(x => x.Value.Count);

        return doesAbbrExist
            ? Sources.StatesAndCities[stateAbbreviation][cityRandomizer.Next(0, max)]
            : Sources.StatesAndCities.Values.SelectMany(x => x).ToList()[cityRandomizer.Next(0, max)];
    }

    /// <summary>Uses the postal code.</summary>
    /// <param name="stateAbbreviation">The state abbreviation.</param>
    /// <param name="useTenChar">if set to <c>true</c> [use ten character].</param>
    /// <returns></returns>
    public static string PostalCode(string stateAbbreviation = null, bool useTenChar = false)
    {
        var doesAbbrExist = !string.IsNullOrWhiteSpace(stateAbbreviation) && Sources.USStatePostalCodeRanges.ContainsKey(stateAbbreviation);

        (int min, int max) range = doesAbbrExist
            ? Sources.USStatePostalCodeRanges[stateAbbreviation]
            : (0, 0);

        if (range.min == 0 && range.max == 0)
        {
            range = (1, 99999);
        }

        var first5 = usPostalRandomizer
            .Next(minValue: range.min, maxValue: range.max)
            .ToString()
            .PadLeft(5, '0')
[..5];

        if (useTenChar == false)
        {
            return first5;
        }

        var lsat4 = usPostalLast4Randomizer
            .Next(minValue: 0, maxValue: 9999)
            .ToString()
            .PadLeft(4, '0');

        return $"{first5}-{lsat4}";
    }

    /// <summary>Numerics the specified minimum length.</summary>
    /// <param name="minLength">The minimum length.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <returns></returns>
    public static string Numeric(int minLength, int maxLength)
    {
        return Generate(
            minLength: minLength,
            maxLength: maxLength,
            CHARS_NUMERIC);
    }

    /// <summary>Chooses the specified items.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static T Choose<T>(IList<T> items)
    {
        if (items == null || items.Count == 0)
        {
            return default;
        }

        if (items.Count == 1)
        {
            return items[0];
        }

        var indexMax = items.Count;
        var index = chooserRandomizer.Next(minValue: 0, maxValue: indexMax);

        return items[index];
    }

    /// <summary>Bools this instance.</summary>
    /// <returns></returns>
    public static bool Bool()
    {
        var number = Generate(
            minLength: 1,
            maxLength: 1,
            CHARS_NUMERIC);

        return number.StartsWith('1') ||
            number.StartsWith('3') ||
            number.StartsWith('5') ||
            number.StartsWith('7') ||
            number.StartsWith('9');
    }

    /// <summary>Phones the number.</summary>
    /// <param name="includeAreaCode">if set to <c>true</c> [include area code].</param>
    /// <param name="format">The format.</param>
    /// <returns></returns>
    public static string PhoneNumber(
        bool? includeAreaCode = null,
        RandomGenPhoneFormat? format = null)
    {
        RandomGenPhoneFormat formatToUse;

        if (!format.HasValue)
        {
            var values = Enum.GetNames<RandomGenPhoneFormat>().ToList();
            var selected = Choose(values);
            formatToUse = Enum.Parse<RandomGenPhoneFormat>(selected, false);
        }
        else
        {
            formatToUse = format.Value;
        }

        var includeAreaCodeToUse = includeAreaCode ?? Bool();

        var numbers = includeAreaCodeToUse
            ? Numeric(10, 10)
            : Numeric(7, 7);

        if (formatToUse == RandomGenPhoneFormat.None)
        {
            return numbers;
        }

        return formatToUse == RandomGenPhoneFormat.Dashes
            ? includeAreaCodeToUse
                ? $"{numbers[..3]}-{numbers.Substring(3, 3)}-{numbers.Substring(6, 4)}"
                : $"{numbers[..3]}-{numbers.Substring(3, 4)}"
            : formatToUse == RandomGenPhoneFormat.Dots
            ? includeAreaCodeToUse
                ? $"{numbers[..3]}.{numbers.Substring(3, 3)}.{numbers.Substring(6, 4)}"
                : $"{numbers[..3]}.{numbers.Substring(3, 4)}"
            : formatToUse == RandomGenPhoneFormat.Full
            ? includeAreaCodeToUse
                ? $"({numbers[..3]}) {numbers.Substring(3, 3)}-{numbers.Substring(6, 4)}"
                : $"{numbers[..3]}-{numbers.Substring(3, 4)}"
            : numbers;
    }
}
