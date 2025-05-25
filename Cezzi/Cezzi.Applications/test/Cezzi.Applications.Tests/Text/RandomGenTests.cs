namespace Cezzi.Applications.Tests.Text;
using Cezzi.Applications.Text;

using Cezzi.Applications.Text.Resources;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

/// <summary>
/// 
/// </summary>
public class RandomGenTests
{
    [Fact]
    public void randomgen___numeric_only_string_min_max_same()
    {
        var generated = RandomGen.Generate(
            minLength: 20,
            maxLength: 20,
            include: RandomGen.CHARS_NUMERIC);

        generated.Should().NotBeNull();
        generated.Should().HaveLength(20);
        generated.Should().MatchRegex(regularExpression: @"^\d+$");
    }

    [Fact]
    public void randomgen___alphanumeric_only_string_min_max_different()
    {
        var generated = RandomGen.Generate(
            minLength: 20,
            maxLength: 30,
            include: [RandomGen.CHARS_NUMERIC, RandomGen.CHARS_LCASE]);

        generated.Should().NotBeNull();
        generated.Length.Should().BeInRange(minimumValue: 20, maximumValue: 30);
        generated.Should().MatchRegex(regularExpression: @"^[a-z0-9]+$");
    }

    [Fact]
    public void randomgen___1000_sequences_all_different()
    {
        var generations = new List<string>();

        for (var i = 0; i < 1000; i++)
        {
            generations.Add(RandomGen.Generate(
                minLength: 20,
                maxLength: 20,
                include: RandomGen.CHARS_NUMERIC));
        }

        generations.Should().HaveCount(1000);
        generations.Distinct().Should().HaveCount(1000);
    }

    [Fact]
    public void randomgen___firstname_success()
    {
        var found = new List<string>();

        for (var i = 0; i < 1000; i++)
        {
            var name = RandomGen.FirstName();
            name.Should().NotBeNullOrWhiteSpace();
            RandomSources.firstnames.Should().Contain(name);
            found.Add(name);
        }

        found.Distinct().Should().HaveCountGreaterThan(20);
    }

    [Fact]
    public void randomgen___lastname_success()
    {
        var found = new List<string>();

        for (var i = 0; i < 1000; i++)
        {
            var name = RandomGen.LastName();
            name.Should().NotBeNullOrWhiteSpace();
            RandomSources.lastnames.Should().Contain(name);
            found.Add(name);
        }

        found.Distinct().Should().HaveCountGreaterThan(20);
    }

    [Fact]
    public void randomgen___streetname_success()
    {
        var found = new List<string>();

        for (var i = 0; i < 1000; i++)
        {
            var name = RandomGen.StreetName();
            name.Should().NotBeNullOrWhiteSpace();
            RandomSources.streetnames.Should().Contain(name);
            found.Add(name);
        }

        found.Distinct().Should().HaveCountGreaterThan(20);
    }

    [Fact]
    public void randomgen___addressandstreetname_success()
    {
        var found = new List<string>();

        var result = Parallel.For(0, 999, (i) =>
        {
            var name = RandomGen.AddressAndStreetName();
            name.Should().NotBeNullOrWhiteSpace();

            Regex.IsMatch(name, "^[0-9]").Should().BeTrue();

            found.Add(name);
        });

        found.Distinct().Should().HaveCountGreaterThan(20);
        result.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void randomgen___statename_success()
    {
        var found = new List<string>();

        for (var i = 0; i < 1000; i++)
        {
            var name = RandomGen.StateAbbr();
            name.Should().NotBeNullOrWhiteSpace();

            Regex.IsMatch(name, "^[A-Z]{2}$").Should().BeTrue();

            found.Add(name);
        }

        found.Distinct().Should().HaveCountGreaterThan(20);
    }

    [Fact]
    public void randomgen___cityname_no_state_success()
    {
        var found = new List<string>();

        for (var i = 0; i < 1000; i++)
        {
            var name = RandomGen.CityName();
            name.Should().NotBeNullOrWhiteSpace();
            name.Should().NotStartWith("#");
            RandomSources.cities.Should().Contain(name);
            found.Add(name);
        }

        found.Distinct().Should().HaveCountGreaterThan(20);
    }

    [Fact]
    public void randomgen___cityname_with_state_success()
    {
        var found = new List<string>();

        for (var i = 0; i < 1000; i++)
        {
            var state = RandomGen.StateAbbr();

            var name = RandomGen.CityName(stateAbbreviation: state);
            name.Should().NotBeNullOrWhiteSpace();
            name.Should().NotStartWith("#");
            RandomSources.cities.Should().Contain(name);
            found.Add(name);
        }

        found.Distinct().Should().HaveCountGreaterThan(20);
    }

    [Fact]
    public void randomgen___fullname_no_middle_success()
    {
        var found = new List<string>();

        for (var i = 0; i < 1000; i++)
        {
            var name = RandomGen.FullName();
            name.Should().NotBeNullOrWhiteSpace();

            var parts = name.Split(' ');
            parts.Length.Should().Be(2);

            RandomSources.firstnames.Should().Contain(parts[0]);
            RandomSources.lastnames.Should().Contain(parts[1]);
            found.Add(name);
        }

        found.Distinct().Should().HaveCountGreaterThan(50);
    }

    [Fact]
    public void randomgen___fullname_has_middle_success()
    {
        var found = new List<string>();

        for (var i = 0; i < 1000; i++)
        {
            var name = RandomGen.FullName(includeMiddleInitial: true);
            name.Should().NotBeNullOrWhiteSpace();

            var parts = name.Split(' ');
            parts.Length.Should().Be(3);

            RandomSources.firstnames.Should().Contain(parts[0]);

            Regex.IsMatch(parts[1], "^[A-Z]{1}$").Should().BeTrue();

            RandomSources.lastnames.Should().Contain(parts[2]);
            found.Add(name);
        }

        found.Distinct().Should().HaveCountGreaterThan(50);
    }

    [Fact]
    public void randomgen___postalcode_no_state_5_char_success()
    {
        for (var i = 0; i < 1000; i++)
        {
            var code = RandomGen.PostalCode();
            code.Should().NotBeNullOrWhiteSpace();
            Regex.IsMatch(code, "^[0-9]{5}$");
        }
    }

    [Fact]
    public void randomgen___postalcode_no_state_10_char_success()
    {
        for (var i = 0; i < 1000; i++)
        {
            var code = RandomGen.PostalCode(useTenChar: true);
            code.Should().NotBeNullOrWhiteSpace();
            Regex.IsMatch(code, "^[0-9]{5}-[0-9]{4}$");
        }
    }

    [Fact]
    public void randomgen___postalcode_with_state_5_char_success()
    {
        for (var i = 0; i < 1000; i++)
        {
            var state = RandomGen.StateAbbr();

            var code = RandomGen.PostalCode(stateAbbreviation: state);
            code.Should().NotBeNullOrWhiteSpace();
            Regex.IsMatch(code, "^[0-9]{5}$");
        }
    }

    [Fact]
    public void randomgen___postalcode_with_state_10_char_success()
    {
        for (var i = 0; i < 1000; i++)
        {
            var state = RandomGen.StateAbbr();

            var code = RandomGen.PostalCode(stateAbbreviation: state, useTenChar: true);
            code.Should().NotBeNullOrWhiteSpace();
            Regex.IsMatch(code, "^[0-9]{5}-[0-9]{4}$");
        }
    }

    [Fact]
    public void randomgen___numeric()
    {
        var number = RandomGen.Numeric(minLength: 1, maxLength: 9);
        number.Length.Should().BeLessThanOrEqualTo(9);
        number.Length.Should().BeGreaterThan(0);

        Convert.ToInt32(number);
    }

    [Fact]
    public void randomgen___choose()
    {
        var names = Enum.GetNames(typeof(RandomGenPhoneFormat));

        var choosenItems = new List<string>();

        for (var i = 0; i < 50; i++)
        {
            var item = RandomGen.Choose(names);
            var val = Enum.Parse<RandomGenPhoneFormat>(item);
            val.ToString().Should().Be(item);

            choosenItems.Add(item);
        }

        choosenItems.Distinct().Should().HaveCountGreaterThan(1);
    }

    [Fact]
    public void randomgen___bool()
    {
        var choosenItems = new List<bool>();

        for (var i = 0; i < 50; i++)
        {
            var item = RandomGen.Bool();
            choosenItems.Add(item);
        }

        choosenItems.Distinct().Should().HaveCount(2);
    }

    [Fact]
    public void randomgen___phone_total_random()
    {
        var phones = new List<string>();

        var tenNumCount = 0;
        var areaFullCount = 0;
        var areaDashCount = 0;
        var areaDotCount = 0;
        var sevenNumCount = 0;
        var sevenDashCount = 0;
        var sevenDotCount = 0;

        for (var i = 0; i < 300; i++)
        {
            var item = RandomGen.PhoneNumber();
            phones.Add(item);
        }

        phones.Distinct().Should().HaveCountGreaterThan(250);

        foreach (var phone in phones)
        {
            if (Regex.IsMatch(phone, @"\d{10}"))
            {
                tenNumCount++;
                continue;
            }

            if (Regex.IsMatch(phone, @"\(\d{3}\) \d{3}-\d{4}"))
            {
                areaFullCount++;
                continue;
            }

            if (Regex.IsMatch(phone, @"\d{3}-\d{3}-\d{4}"))
            {
                areaDashCount++;
                continue;
            }

            if (Regex.IsMatch(phone, @"\d{3}\.\d{3}\.\d{4}"))
            {
                areaDotCount++;
                continue;
            }

            if (Regex.IsMatch(phone, @"\d{7}"))
            {
                sevenNumCount++;
                continue;
            }

            if (Regex.IsMatch(phone, @"\d{3}-\d{4}"))
            {
                sevenDashCount++;
                continue;
            }

            if (Regex.IsMatch(phone, @"\d{3}\.\d{4}"))
            {
                sevenDotCount++;
                continue;
            }

            Assert.Fail("phone returned an invalid format");
        }

        tenNumCount.Should().BePositive();
        areaFullCount.Should().BePositive();
        areaDashCount.Should().BePositive();
        areaDotCount.Should().BePositive();
        sevenNumCount.Should().BePositive();
        sevenDashCount.Should().BePositive();
        sevenDotCount.Should().BePositive();
    }

    [Fact]
    public void randomgen___phone_areacode_full()
    {
        var phone = RandomGen.PhoneNumber(includeAreaCode: true, format: RandomGenPhoneFormat.Full);

        Regex.IsMatch(phone, @"\(\d{3}\) \d{3}-\d{4}").Should().BeTrue();
    }

    [Fact]
    public void randomgen___phone_full()
    {
        var phone = RandomGen.PhoneNumber(includeAreaCode: false, format: RandomGenPhoneFormat.Full);

        Regex.IsMatch(phone, @"\d{3}-\d{4}").Should().BeTrue();
    }

    [Fact]
    public void randomgen___phone_area_dashes()
    {
        var phone = RandomGen.PhoneNumber(includeAreaCode: true, format: RandomGenPhoneFormat.Dashes);

        Regex.IsMatch(phone, @"\d{3}-\d{3}-\d{4}").Should().BeTrue();
    }

    [Fact]
    public void randomgen___phone_dashes()
    {
        var phone = RandomGen.PhoneNumber(includeAreaCode: false, format: RandomGenPhoneFormat.Dashes);

        Regex.IsMatch(phone, @"\d{3}-\d{4}").Should().BeTrue();
    }

    [Fact]
    public void randomgen___phone_area_dots()
    {
        var phone = RandomGen.PhoneNumber(includeAreaCode: true, format: RandomGenPhoneFormat.Dots);

        Regex.IsMatch(phone, @"\d{3}\.\d{3}\.\d{4}").Should().BeTrue();
    }

    [Fact]
    public void randomgen___phone_dots()
    {
        var phone = RandomGen.PhoneNumber(includeAreaCode: false, format: RandomGenPhoneFormat.Dots);

        Regex.IsMatch(phone, @"\d{3}\.\d{4}").Should().BeTrue();
    }

    [Fact]
    public void randomgen___phone_area_none()
    {
        var phone = RandomGen.PhoneNumber(includeAreaCode: true, format: RandomGenPhoneFormat.None);

        Regex.IsMatch(phone, @"\d{10}").Should().BeTrue();
    }

    [Fact]
    public void randomgen___phone_none()
    {
        var phone = RandomGen.PhoneNumber(includeAreaCode: false, format: RandomGenPhoneFormat.None);

        Regex.IsMatch(phone, @"\d{7}").Should().BeTrue();
    }
}
