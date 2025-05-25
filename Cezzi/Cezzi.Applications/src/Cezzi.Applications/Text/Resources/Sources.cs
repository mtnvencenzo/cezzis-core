namespace Cezzi.Applications.Text.Resources;

using System.Collections.Generic;
using System.IO;

internal static class Sources
{
    private static IList<string> firstNames;
    private readonly static object firstNamesLock = new();
    private static IList<string> lastNames;
    private readonly static object lastNamesLock = new();
    private static IList<string> streetNames;
    private readonly static object streetNamesLock = new();
    private static IDictionary<string, IList<string>> statesAndCities;
    private readonly static object statesAndCitiesLock = new();
    private static IDictionary<string, (int min, int max)> usStatesPostalCodeRanges;
    private readonly static object usStatesPostalCodeRangesLock = new();

    internal static IDictionary<string, (int min, int max)> USStatePostalCodeRanges => GetUSPostalCodeRanges();

    internal static IList<string> FirstNames => GetFirstNames();

    internal static IList<string> LastNames => GetLastNames();

    internal static IList<string> StreetNames => GetStreetNames();

    internal static IDictionary<string, IList<string>> StatesAndCities => GetStatesAndCities();

    private static IList<string> GetFirstNames()
    {
        if (firstNames == null)
        {
            lock (firstNamesLock)
            {
                if (firstNames == null)
                {
                    var buffer = new List<string>();

                    using (var reader = new StringReader(RandomSources.firstnames))
                    {
                        while (true)
                        {
                            var line = reader.ReadLine();
                            if (line == null)
                            {
                                break;
                            }

                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                buffer.Add(line.Trim());
                            }
                        }
                    }

                    firstNames = buffer;
                }
            }
        }

        return firstNames;
    }

    private static IList<string> GetLastNames()
    {
        if (lastNames == null)
        {
            lock (lastNamesLock)
            {
                if (lastNames == null)
                {
                    var buffer = new List<string>();

                    using (var reader = new StringReader(RandomSources.lastnames))
                    {
                        while (true)
                        {
                            var line = reader.ReadLine();
                            if (line == null)
                            {
                                break;
                            }

                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                buffer.Add(line.Trim());
                            }
                        }
                    }

                    lastNames = buffer;
                }
            }
        }

        return lastNames;
    }

    private static IList<string> GetStreetNames()
    {
        if (streetNames == null)
        {
            lock (streetNamesLock)
            {
                if (streetNames == null)
                {
                    var buffer = new List<string>();

                    using (var reader = new StringReader(RandomSources.streetnames))
                    {
                        while (true)
                        {
                            var line = reader.ReadLine();
                            if (line == null)
                            {
                                break;
                            }

                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                buffer.Add(line.Trim());
                            }
                        }
                    }

                    streetNames = buffer;
                }
            }
        }

        return streetNames;
    }

    private static IDictionary<string, IList<string>> GetStatesAndCities()
    {
        if (statesAndCities == null)
        {
            lock (statesAndCitiesLock)
            {
                if (statesAndCities == null)
                {
                    var buffer = new Dictionary<string, IList<string>>();

                    using (var reader = new StringReader(RandomSources.cities))
                    {
                        var currentStateAbbr = string.Empty;

                        while (true)
                        {
                            var line = reader.ReadLine();
                            if (line == null)
                            {
                                break;
                            }

                            if (line.StartsWith("#"))
                            {
                                currentStateAbbr = line.Split('-')[1].Trim();
                                buffer.Add(currentStateAbbr, []);
                                continue;
                            }

                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                buffer[currentStateAbbr].Add(line.Trim());
                            }
                        }
                    }

                    statesAndCities = buffer;
                }
            }
        }

        return statesAndCities;
    }

    private static IDictionary<string, (int min, int max)> GetUSPostalCodeRanges()
    {
        if (usStatesPostalCodeRanges == null)
        {
            lock (usStatesPostalCodeRangesLock)
            {
                usStatesPostalCodeRanges ??= new Dictionary<string, (int min, int max)>
                {
                    { nameof(UsPostalCodeRanges.AK), GetPostalRange(UsPostalCodeRanges.AK) },
                    { nameof(UsPostalCodeRanges.AL), GetPostalRange(UsPostalCodeRanges.AL) },
                    { nameof(UsPostalCodeRanges.AR), GetPostalRange(UsPostalCodeRanges.AR) },
                    { nameof(UsPostalCodeRanges.AZ), GetPostalRange(UsPostalCodeRanges.AZ) },
                    { nameof(UsPostalCodeRanges.CA), GetPostalRange(UsPostalCodeRanges.CA) },
                    { nameof(UsPostalCodeRanges.CO), GetPostalRange(UsPostalCodeRanges.CO) },
                    { nameof(UsPostalCodeRanges.CT), GetPostalRange(UsPostalCodeRanges.CT) },
                    { nameof(UsPostalCodeRanges.DC), GetPostalRange(UsPostalCodeRanges.DC) },
                    { nameof(UsPostalCodeRanges.DE), GetPostalRange(UsPostalCodeRanges.DE) },
                    { nameof(UsPostalCodeRanges.FL), GetPostalRange(UsPostalCodeRanges.FL) },
                    { nameof(UsPostalCodeRanges.GA), GetPostalRange(UsPostalCodeRanges.GA) },
                    { nameof(UsPostalCodeRanges.HI), GetPostalRange(UsPostalCodeRanges.HI) },
                    { nameof(UsPostalCodeRanges.IA), GetPostalRange(UsPostalCodeRanges.IA) },
                    { nameof(UsPostalCodeRanges.ID), GetPostalRange(UsPostalCodeRanges.ID) },
                    { nameof(UsPostalCodeRanges.IL), GetPostalRange(UsPostalCodeRanges.IL) },
                    { nameof(UsPostalCodeRanges.IN), GetPostalRange(UsPostalCodeRanges.IN) },
                    { nameof(UsPostalCodeRanges.KS), GetPostalRange(UsPostalCodeRanges.KS) },
                    { nameof(UsPostalCodeRanges.KY), GetPostalRange(UsPostalCodeRanges.KY) },
                    { nameof(UsPostalCodeRanges.LA), GetPostalRange(UsPostalCodeRanges.LA) },
                    { nameof(UsPostalCodeRanges.MA), GetPostalRange(UsPostalCodeRanges.MA) },
                    { nameof(UsPostalCodeRanges.MD), GetPostalRange(UsPostalCodeRanges.MD) },
                    { nameof(UsPostalCodeRanges.ME), GetPostalRange(UsPostalCodeRanges.ME) },
                    { nameof(UsPostalCodeRanges.MI), GetPostalRange(UsPostalCodeRanges.MI) },
                    { nameof(UsPostalCodeRanges.MN), GetPostalRange(UsPostalCodeRanges.MN) },
                    { nameof(UsPostalCodeRanges.MO), GetPostalRange(UsPostalCodeRanges.MO) },
                    { nameof(UsPostalCodeRanges.MS), GetPostalRange(UsPostalCodeRanges.MS) },
                    { nameof(UsPostalCodeRanges.MT), GetPostalRange(UsPostalCodeRanges.MT) },
                    { nameof(UsPostalCodeRanges.NC), GetPostalRange(UsPostalCodeRanges.NC) },
                    { nameof(UsPostalCodeRanges.ND), GetPostalRange(UsPostalCodeRanges.ND) },
                    { nameof(UsPostalCodeRanges.NE), GetPostalRange(UsPostalCodeRanges.NE) },
                    { nameof(UsPostalCodeRanges.NH), GetPostalRange(UsPostalCodeRanges.NH) },
                    { nameof(UsPostalCodeRanges.NJ), GetPostalRange(UsPostalCodeRanges.NJ) },
                    { nameof(UsPostalCodeRanges.NM), GetPostalRange(UsPostalCodeRanges.NM) },
                    { nameof(UsPostalCodeRanges.NV), GetPostalRange(UsPostalCodeRanges.NV) },
                    { nameof(UsPostalCodeRanges.NY), GetPostalRange(UsPostalCodeRanges.NY) },
                    { nameof(UsPostalCodeRanges.OH), GetPostalRange(UsPostalCodeRanges.OH) },
                    { nameof(UsPostalCodeRanges.OK), GetPostalRange(UsPostalCodeRanges.OK) },
                    { nameof(UsPostalCodeRanges.OR), GetPostalRange(UsPostalCodeRanges.OR) },
                    { nameof(UsPostalCodeRanges.PA), GetPostalRange(UsPostalCodeRanges.PA) },
                    { nameof(UsPostalCodeRanges.RI), GetPostalRange(UsPostalCodeRanges.RI) },
                    { nameof(UsPostalCodeRanges.SC), GetPostalRange(UsPostalCodeRanges.SC) },
                    { nameof(UsPostalCodeRanges.SD), GetPostalRange(UsPostalCodeRanges.SD) },
                    { nameof(UsPostalCodeRanges.TN), GetPostalRange(UsPostalCodeRanges.TN) },
                    { nameof(UsPostalCodeRanges.TX), GetPostalRange(UsPostalCodeRanges.TX) },
                    { nameof(UsPostalCodeRanges.UT), GetPostalRange(UsPostalCodeRanges.UT) },
                    { nameof(UsPostalCodeRanges.VA), GetPostalRange(UsPostalCodeRanges.VA) },
                    { nameof(UsPostalCodeRanges.VT), GetPostalRange(UsPostalCodeRanges.VT) },
                    { nameof(UsPostalCodeRanges.WA), GetPostalRange(UsPostalCodeRanges.WA) },
                    { nameof(UsPostalCodeRanges.WI), GetPostalRange(UsPostalCodeRanges.WI) },
                    { nameof(UsPostalCodeRanges.WV), GetPostalRange(UsPostalCodeRanges.WV) },
                    { nameof(UsPostalCodeRanges.WY), GetPostalRange(UsPostalCodeRanges.WY) }
                };
            }
        }

        return usStatesPostalCodeRanges;
    }

    private static (int min, int max) GetPostalRange(string range)
    {
        if (string.IsNullOrWhiteSpace(range))
        {
            return (0, 0);
        }

        var rangeParts = range.Split('_');
        if (rangeParts.Length != 2)
        {
            return (0, 0);
        }

        if (int.TryParse(rangeParts[0], out var min) && int.TryParse(rangeParts[1], out var max))
        {
            if (min <= max)
            {
                return (min, max);
            }
        }

        return (0, 0);
    }
}
