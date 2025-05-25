namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using System;
using Xunit;

public class EnumExtensions_Tests
{
    // Single AsEnum<> (non-flagged)
    // --------------------

    [Fact]
    public void enumextensions___asenum_single_returns_default_when_cant_convert()
    {
        var i = -1;

        var result = i.AsEnum<FonzieEnum>();
        result.Should().Be((FonzieEnum)(-1));
    }

    [Theory]
    [InlineData("None", FonzieEnum.None)]
    [InlineData("Ayyy", FonzieEnum.Ayyy)]
    [InlineData("ayyy", FonzieEnum.Ayyy)]
    [InlineData("Ravioli", FonzieEnum.Ravioli)]
    [InlineData("RavioLI", FonzieEnum.Ravioli)]
    public void enumextensions___asenum_single_returns_true_case_insensitive(string str, FonzieEnum expected)
    {
        var result = str.AsEnum<FonzieEnum>(true);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("none", FonzieEnum.None)]
    [InlineData("not-real", FonzieEnum.None)]
    [InlineData("ayyy", FonzieEnum.None)]
    [InlineData("ravioli", FonzieEnum.None)]
    public void enumextensions___asenum_single_returns_false_case_insensitive(string str, FonzieEnum expected)
    {
        var result = str.AsEnum<FonzieEnum>(false);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Ayyy", "", FonzieEnum.Ayyy)]
    [InlineData("RavioLI", "", FonzieEnum.Ravioli)]
    [InlineData("Cannoli-Penne", "", FonzieEnum.Cannoli_Penne)]
    [InlineData("Cannoli-Penne", " ", FonzieEnum.Cannoli_Penne)]
    [InlineData("Cannoli-Penne", null, FonzieEnum.Cannoli_Penne)]
    [InlineData("Cannoli-Penne", "_", FonzieEnum.Cannoli_Penne)]
    [InlineData("Cannoli Penne", "", FonzieEnum.Cannoli_Penne)]
    [InlineData("Cannoli Penne", " ", FonzieEnum.Cannoli_Penne)]
    [InlineData("Cannoli Penne", null, FonzieEnum.Cannoli_Penne)]
    public void enumextensions___asenum_single_with_replacement_returns_true_case_insensitive(string str, string replacement, FonzieEnum expected)
    {
        var result = str.AsEnum<FonzieEnum>(true, replacement);
        result.Should().Be(expected);
    }

    [Fact]
    public void enumextensions___asenum_throws_when_not_enum()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            var resutl = "test".AsEnum<int>(true);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("T must be an enum type");
    }

    // Single AsEnum<> (flagged)
    // --------------------

    [Theory]
    [InlineData("Ayyy,Cannoli_Penne", FonzieFlaggedEnum.Ayyy | FonzieFlaggedEnum.Cannoli_Penne)]
    [InlineData("Ayyy,Cannoli_Penne,Ravioli", FonzieFlaggedEnum.Ayyy | FonzieFlaggedEnum.Cannoli_Penne | FonzieFlaggedEnum.Ravioli)]
    [InlineData("Ayyy", FonzieFlaggedEnum.Ayyy)]
    [InlineData("Ayyy,None", FonzieFlaggedEnum.Ayyy | FonzieFlaggedEnum.None)]
    public void enumextensions___asenum_flagged_returns_true_case_insensitive(string str, FonzieFlaggedEnum expected)
    {
        var result = str.AsEnum<FonzieFlaggedEnum>(true);
        result.Should().Be(expected);
    }

    // Single AsString<> (non-flagged)
    // --------------------

    [Fact]
    public void enumextensions___asstring_single_throws_if_not_enum()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            var i = -1;
            var resutl = i.AsString();
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("T must be an enum type");
    }

    [Theory]
    [InlineData("Ayyy", FonzieEnum.Ayyy)]
    [InlineData("None", FonzieEnum.None)]
    [InlineData("Ravioli", FonzieEnum.Ravioli)]
    [InlineData("Cannoli Penne", FonzieEnum.Cannoli_Penne)]
    public void enumextensions___asstring_single_works(string expected, FonzieEnum fonzie)
    {
        var result = fonzie.AsString();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Cannoli Penne", null, FonzieEnum.Cannoli_Penne)]
    [InlineData("Cannoli Penne", "", FonzieEnum.Cannoli_Penne)]
    [InlineData("Cannoli Penne", " ", FonzieEnum.Cannoli_Penne)]
    [InlineData("Cannoli_Penne", "_", FonzieEnum.Cannoli_Penne)]
    [InlineData("Cannoli-Penne", "-", FonzieEnum.Cannoli_Penne)]
    public void enumextensions___asstring_single_works_with_replacement(string expected, string replacement, FonzieEnum fonzie)
    {
        var result = fonzie.AsString(replacement);
        result.Should().Be(expected);
    }

    // Single GetAttributeOfType<>
    // --------------------

    [Fact]
    public void enumextensions___getattributeoftype_for_member_with_attribute()
    {
        var result = FonzieEnum.Ravioli.GetAttributeOfType<EnumMemberAttribute>();

        result.Should().NotBeNull();
        result.Value.Should().Be("test");
    }

    [Fact]
    public void enumextensions___getattributeoftype_for_member_without_attribute()
    {
        var result = FonzieEnum.Ayyy.GetAttributeOfType<EnumMemberAttribute>();

        result.Should().BeNull();
    }

    // Single ToCommaSeparatedString<>
    // --------------------

    [Fact]
    public void enumextensions___tocommasepstring_single() => FonzieEnum.Ravioli.ToCommaSeparatedString().Should().Be(nameof(FonzieEnum.Ravioli));

    [Fact]
    public void enumextensions___tocommasepstring_flagged_but_single() => FonzieFlaggedEnum.Ravioli.ToCommaSeparatedString().Should().Be(nameof(FonzieFlaggedEnum.Ravioli));

    [Fact]
    public void enumextensions___tocommasepstring_flagged_but_multi()
    {
        var val = FonzieFlaggedEnum.Ravioli | FonzieFlaggedEnum.Ayyy;
        val.ToCommaSeparatedString().Should().Be($"{FonzieFlaggedEnum.Ayyy},{FonzieFlaggedEnum.Ravioli}");

        val = FonzieFlaggedEnum.Cannoli_Penne | FonzieFlaggedEnum.Ravioli | FonzieFlaggedEnum.None;
        val.ToCommaSeparatedString().Should().Be($"{FonzieFlaggedEnum.Ravioli},{FonzieFlaggedEnum.Cannoli_Penne}");

        val = FonzieFlaggedEnum.Cannoli_Penne | FonzieFlaggedEnum.Ravioli | FonzieFlaggedEnum.None | FonzieFlaggedEnum.Ayyy;
        val.ToCommaSeparatedString().Should().Be($"{FonzieFlaggedEnum.Ayyy},{FonzieFlaggedEnum.Ravioli},{FonzieFlaggedEnum.Cannoli_Penne}");
    }

    [Fact]
    public void enumextensions___tocommasepstring_throws_when_not_enum()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            var i = -1;
            var resutl = i.ToCommaSeparatedString();
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("enumType must be an enum");
    }

    // Single ToCommaSeparatedString<>
    // --------------------

    [Fact]
    public void enumextensions___issingleflaggedenum_for_single_non_flagged() => FonzieEnum.Ravioli.IsSingleFlaggedEnumMember().Should().BeTrue();

    [Fact]
    public void enumextensions___issingleflaggedenum_for_single_flagged() => FonzieFlaggedEnum.Ravioli.IsSingleFlaggedEnumMember().Should().BeTrue();

    [Fact]
    public void enumextensions___issingleflaggedenum_for_multi_flagged() => (FonzieFlaggedEnum.Ravioli | FonzieFlaggedEnum.Cannoli_Penne).IsSingleFlaggedEnumMember().Should().BeFalse();

    [Fact]
    public void enumextensions___issingleflaggedenum_throws_when_not_enum()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            1.IsSingleFlaggedEnumMember().Should().BeTrue();
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("enumType must be an enum");
    }

    // Single ParseCommaSeparatedFlagedEnum<>
    // --------------------

    [Fact]
    public void enumextensions___parsecommaseparated_throws_when_not_enum()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            EnumExtensions.ParseCommaSeparatedFlagedEnum<int>("");
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("types must be an enum type");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void enumextensions___parsecommaseparated_returns_default_for_empty_string(string val)
    {
        var result = EnumExtensions.ParseCommaSeparatedFlagedEnum<FonzieFlaggedEnum>(val);
        result.Should().Be(default);
    }

    [Fact]
    public void enumextensions___parsecommaseparated_returns_with_item_processor()
    {
        var result = EnumExtensions.ParseCommaSeparatedFlagedEnum<FonzieFlaggedEnum>("test", true, (str) => nameof(FonzieEnum.Ravioli));
        result.Should().Be(FonzieFlaggedEnum.Ravioli);
    }

    public enum FonzieEnum
    {
        None = 0,

        Ayyy = 1,

        [EnumMember("test")]
        Ravioli = 2,

        Cannoli_Penne
    }

    [Flags]
    public enum FonzieFlaggedEnum
    {
        None = 0,

        Ayyy = 1,

        Ravioli = 2,

        Cannoli_Penne = 4
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class EnumMemberAttribute(string value) : Attribute
    {
        public string Value { get; } = value;
    }
}
