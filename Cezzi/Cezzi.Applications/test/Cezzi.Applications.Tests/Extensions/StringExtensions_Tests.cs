namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using System;
using Xunit;

public class StringExtensions_Tests
{
    // NullIfNullOrWhiteSpace
    // ----------------------------

    [Fact]
    public void stringextensions___NullIfNullOrWhiteSpace_returns_null_if_string_is_null()
    {
        string s = null;

        var result = s.NullIfNullOrWhiteSpace();

        result.Should().BeNull();
    }

    [Fact]
    public void stringextensions___NullIfNullOrWhiteSpace_returns_null_if_string_is_whitespace()
    {
        var s = " ";

        var result = s.NullIfNullOrWhiteSpace();

        result.Should().BeNull();
    }

    [Fact]
    public void stringextensions___NullIfNullOrWhiteSpace_returns_null_if_string_is_empty()
    {
        var s = "";

        var result = s.NullIfNullOrWhiteSpace();

        result.Should().BeNull();
    }

    [Fact]
    public void stringextensions___NullIfNullOrWhiteSpace_returns_string_if_string_is_not_null_or_whitespace()
    {
        var s = " fd fvfsdf sdfjsd osd s dif s dif  sd f ii sdf ";

        var result = s.NullIfNullOrWhiteSpace();

        result.Should().Be(s);
    }

    // AllAreNullOrWhiteSpace
    // ----------------------------

    [Fact]
    public void stringextensions___AllAreNullOrWhiteSpace_throws_if_parmas_are_null()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            paramName: "strings",
            testCode: () => StringExtensions.AllAreNullOrWhiteSpace(null));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void stringextensions___AllAreNullOrWhiteSpace_returns_true_if_one_is_null()
    {
        var result = StringExtensions.AllAreNullOrWhiteSpace(null as string);

        result.Should().BeTrue();
    }

    [Fact]
    public void stringextensions___AllAreNullOrWhiteSpace_returns_true_if_all_are_null_or_whitespace()
    {
        var result = StringExtensions.AllAreNullOrWhiteSpace(null, "", " ", null, string.Empty);

        result.Should().BeTrue();
    }

    [Fact]
    public void stringextensions___AllAreNullOrWhiteSpace_returns_false_if_one_of_is_not_null_or_whitespace()
    {
        var result = StringExtensions.AllAreNullOrWhiteSpace(null, "", " ", null, "test", string.Empty);

        result.Should().BeFalse();
    }

    [Fact]
    public void stringextensions___AllAreNullOrWhiteSpace_returns_false_if_single_is_not_null_or_whitespace()
    {
        var result = StringExtensions.AllAreNullOrWhiteSpace("test");

        result.Should().BeFalse();
    }

    // AllAreNotNullOrWhiteSpace
    // ----------------------------

    [Fact]
    public void stringextensions___AllAreNotNullOrWhiteSpace_throws_if_parmas_are_null()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            paramName: "strings",
            testCode: () => StringExtensions.AllAreNotNullOrWhiteSpace(null));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void stringextensions___AllAreNotNullOrWhiteSpace_returns_false_if_one_is_null()
    {
        var result = StringExtensions.AllAreNotNullOrWhiteSpace(null as string);

        result.Should().BeFalse();
    }

    [Fact]
    public void stringextensions___AllAreNotNullOrWhiteSpace_returns_true_if_all_are_not_null_or_whitespace()
    {
        var result = StringExtensions.AllAreNotNullOrWhiteSpace("a", "b", " b", "d ", "_");

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void stringextensions___AllAreNotNullOrWhiteSpace_returns_false_if_one_of_is_null_or_whitespace(string checkVal)
    {
        var result = StringExtensions.AllAreNotNullOrWhiteSpace("a", "h", " f", "e", "test", checkVal);

        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void stringextensions___AllAreNotNullOrWhiteSpace_returns_false_if_single_is_null_or_whitespace(string checkVal)
    {
        var result = StringExtensions.AllAreNotNullOrWhiteSpace(checkVal);

        result.Should().BeFalse();
    }

    // Truncate
    // ----------------------------

    [Fact]
    public void stringextensions__truncate_returns_null_for_null_string()
    {
        var result = (null as string).Truncate(1);

        result.Should().BeNull();
    }

    [Fact]
    public void stringextensions__truncate_returns_correct_value_when_all_whitespace()
    {
        var result = "          ".Truncate(5);

        result.Should().Be("     ");
    }

    [Fact]
    public void stringextensions__truncate_returns_correct_value_when_truncate_length_greater_than_actual_length()
    {
        var result = "12345".Truncate(10);

        result.Should().Be("12345");
    }

    [Fact]
    public void stringextensions__truncate_returns_correct_value_when_asome_whitespace()
    {
        var result = "   asd   ".Truncate(8);

        result.Should().Be("   asd  ");
    }

    // TrimSafe
    // ----------------------------

    [Fact]
    public void stringextensions__trimsafe_returns_null_for_null_string()
    {
        var result = (null as string).TrimSafe();

        result.Should().BeNull();
    }

    [Fact]
    public void stringextensions__trimsafe_returns_trimmed_string()
    {
        var result = "   djdj k dk doosos dd       ".TrimSafe();

        result.Should().Be("djdj k dk doosos dd");
    }

    // RemoveAllWhiteSpace
    // ----------------------------

    [Fact]
    public void stringextensions__removeallwhitespace_returns_null_for_null_string()
    {
        var result = (null as string).RemoveAllWhiteSpace();

        result.Should().BeNull();
    }

    [Fact]
    public void stringextensions__removeallwhitespace_returns_no_whitespace_for_whitespace_only_string()
    {
        var result = " ".RemoveAllWhiteSpace();

        result.Should().Be("");
    }

    [Fact]
    public void stringextensions__removeallwhitespace_returns_no_whitespace_for_empty_string()
    {
        var result = "".RemoveAllWhiteSpace();

        result.Should().Be("");
    }

    [Fact]
    public void stringextensions__removeallwhitespace_returns_no_whitespace()
    {
        var result = " kk s oe emmfk  fkk  kkf  ".RemoveAllWhiteSpace();

        result.Should().Be("kksoeemmfkfkkkkf");
    }

    // IsNumbersOnly
    // ----------------------------

    [Fact]
    public void stringextensions__isnumbersonly_throws_for_null_string()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            paramName: "input",
            testCode: () => (null as string).IsNumbersOnly());

        ex.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void stringextensions__isnumbersonly_returns_false_for_empty_or_whitespace(string input)
    {
        var result = input.IsNumbersOnly();

        result.Should().BeFalse();
    }

    [Fact]
    public void stringextensions__isnumbersonly_returns_false_for_non_numbers()
    {
        var result = "912912399109_233".IsNumbersOnly();

        result.Should().BeFalse();
    }

    [Fact]
    public void stringextensions__isnumbersonly_returns_true_for_numbers_only()
    {
        var result = "912912399109233".IsNumbersOnly();

        result.Should().BeTrue();
    }

    // RemoveAllNonNumbers
    // ----------------------------

    [Fact]
    public void stringextensions__removeallnonnumbers_returns_null_for_null_string()
    {
        var result = (null as string).RemoveAllNonNumbers();

        result.Should().BeNull();
    }

    [Fact]
    public void stringextensions__removeallnonnumbers_returns_no_numbers_for_mixed_only_char_string()
    {
        var result = @" a fjaj f- f @)(#*@!_j jadkfjacnnoieurweoiwu][;'/.,qw\w][eojldknzx,mbnmvbb~`\+_)(*&^%$#@!".RemoveAllNonNumbers();

        result.Should().Be("");
    }

    [Fact]
    public void stringextensions__removeallnonnumbers_returns_no_numbers_for_empty_string()
    {
        var result = "".RemoveAllNonNumbers();

        result.Should().Be("");
    }

    [Fact]
    public void stringextensions__removeallnonnumbers_returns_only_numbers()
    {
        var result = @" a fjaj f-1 f @)(#*@!_j jadkf2jacnnoieurweoiwu3][;'/.,qw\w][eojldk4nzx,mbnmv5bb67~`8\+_)(*&^%9$#@!0".RemoveAllNonNumbers();

        result.Should().Be("1234567890");
    }

    // SubstringSafe
    // ----------------------------

    [Theory]
    [InlineData(null, 0, 1, null)]
    [InlineData("", 0, 1, "")]
    [InlineData(null, null, null, null)]
    [InlineData("abcdef", null, null, "abcdef")]
    [InlineData("abcdef", 1, null, "bcdef")]
    [InlineData("abcdef", 5, null, "f")]
    [InlineData("abcdef", 6, null, "")]
    [InlineData("abcdef", 7, null, "")]
    [InlineData("abcdef", -1, null, "abcdef")]
    [InlineData("abcdef", null, 1, "a")]
    [InlineData("abcdef", null, 9, "abcdef")]
    [InlineData("abcdef", 1, 1, "b")]
    [InlineData("abcdef", 2, -1, "")]
    [InlineData("abcdef", 6, 1, "")]
    [InlineData("abcdef", 6, 2, "")]
    [InlineData("abcdef", 7, 2, "")]
    public void stringextensions__SubstringSafe(string inString, int? start, int? length, string expected)
    {
        var result = start == null && length == null
            ? inString.SubstringSafe()
            : start == null
                ? inString.SubstringSafe(length: length)
                : length == null
                            ? inString.SubstringSafe(startIndex: start.Value)
                            : inString.SubstringSafe(startIndex: start.Value, length: length);
        result.Should().Be(expected);
    }

    // InLength
    // ----------------------------

    [Fact]
    public void stringextensions__inlength_throws_when_min_lessthan_zero()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            "".InLength(-1, 0);
        });

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("minLength");
        ex.ActualValue.Should().Be(-1);
        ex.Message.Should().StartWith("Cannot be less than zero(0)");
    }

    [Fact]
    public void stringextensions__inlength_throws_when_max_lessthan_min()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            "".InLength(11, 10);
        });

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("maxLength");
        ex.ActualValue.Should().Be(10);
        ex.Message.Should().StartWith("Cannot be less than minLength");
    }

    [Fact]
    public void stringextensions__inlength_returns_true_when_allownull_and_0_length() => (null as string).InLength(0, 1, true).Should().BeTrue();

    [Fact]
    public void stringextensions__inlength_returns_false_when_not_allownull_and_0_length() => (null as string).InLength(0, 1, false).Should().BeFalse();

    [Theory]
    [InlineData("", 1, 1)]
    [InlineData("test", 5, 10)]
    [InlineData("test", 0, 3)]
    public void stringextensions__inlength_returns_false(string str, int min, int max) => str.InLength(min, max).Should().BeFalse();

    [Theory]
    [InlineData("", 0, 1)]
    [InlineData(" ", 1, 1)]
    [InlineData("test", 4, 10)]
    [InlineData("test", 1, 4)]
    public void stringextensions__inlength_returns_true(string str, int min, int max) => str.InLength(min, max).Should().BeTrue();

    // IgnoreCaseContains
    // -------------

    [Theory]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData(" ", "")]
    [InlineData(" ", " ")]
    [InlineData("", null)]
    [InlineData("test", "es")]
    [InlineData("test", "test")]
    [InlineData("test", "t")]
    [InlineData("test", "Es")]
    [InlineData("test", "Test")]
    [InlineData("test", "T")]
    public void stringextensions__ignorecasecontains_returns_true(string str, string containing) => str.IgnoreCaseContains(containing).Should().BeTrue();

    [Theory]
    [InlineData(null, " ")]
    [InlineData("test", "somethingelse")]
    [InlineData("test", "a")]
    public void stringextensions__ignorecasecontains_returns_false(string str, string containing) => str.IgnoreCaseContains(containing).Should().BeFalse();

    // IsNumeric
    // -------------

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("a1")]
    [InlineData("1.1")]
    [InlineData("-1")]
    public void stringextensions__isnumeric_returns_false(string str) => str.IsNumeric().Should().BeFalse();

    [Theory]
    [InlineData("1")]
    [InlineData("00000001")]
    [InlineData("000001")]
    [InlineData("12")]
    [InlineData("0")]
    public void stringextensions__isnumeric_returns_true(string str) => str.IsNumeric().Should().BeTrue();

    // TrimAndTruncate
    // -------------

    [Theory]
    [InlineData(null, 0, null)]
    [InlineData(null, 1, null)]
    [InlineData(" ", 0, "")]
    [InlineData(" ", 1, "")]
    [InlineData("", 0, "")]
    [InlineData("", 1, "")]
    [InlineData("test", 1, "t")]
    [InlineData("tester", 20, "tester")]
    [InlineData(" tester ", 20, "tester")]
    [InlineData("tester ", 4, "test")]
    public void stringextensions__trimandtruncate_returns_false(string str, int length, string expected) => str.TrimAndTruncate(length).Should().Be(expected);

    // IsIn
    // -------------

    [Theory]
    [InlineData("test", "a", "b", "test")]
    [InlineData("test", "a", "Test", "Test")]
    [InlineData("test", "Test")]
    [InlineData("test", "test")]
    public void stringextensions__isin_case_insensitive_returns_true(string str, params string[] initems) => StringExtensions.IsIn(str, initems, true).Should().BeTrue();

    [Theory]
    [InlineData("test", "a", "b", "test")]
    [InlineData("test", "a", "Test", "test")]
    [InlineData("test", "test")]
    public void stringextensions__isin_case_sensitive_returns_true(string str, params string[] initems) => StringExtensions.IsIn(str, initems, false).Should().BeTrue();

    [Theory]
    [InlineData("test", "a", "b", "atest")]
    [InlineData("test", "a", "Taest", "Tesst")]
    [InlineData("test", "Tesat")]
    [InlineData("test", "tesat")]
    public void stringextensions__isin_case_insensitive_returns_false(string str, params string[] initems) => StringExtensions.IsIn(str, initems, true).Should().BeFalse();

    [Theory]
    [InlineData("test", "a", "b", "Test")]
    [InlineData("test", "a", "Test", "Test")]
    [InlineData("test", "Test")]
    [InlineData("test", "tEst")]
    public void stringextensions__isin_case_sensitive_returns_false(string str, params string[] initems) => StringExtensions.IsIn(str, initems, false).Should().BeFalse();

    [Fact]
    public void stringextensions__isin_returns_false_for_null_string() => StringExtensions.IsIn(null, ["test"], false).Should().BeFalse();

    [Fact]
    public void stringextensions__isin_returns_false_for_null_list() => StringExtensions.IsIn("", null, false).Should().BeFalse();

    // Left
    // -------------

    [Theory]
    [InlineData(null, 1, "")]
    [InlineData("", 1, "")]
    [InlineData("test", 1, "t")]
    [InlineData(" test", 1, " ")]
    [InlineData("test", 100, "test")]
    [InlineData(" test", 100, " test")]
    [InlineData(" test", 3, " te")]
    public void stringextensions__left_returns_expected(string str, int length, string expected) => str.Left(length).Should().Be(expected);

    // Right
    // -------------

    [Theory]
    [InlineData(null, 1, "")]
    [InlineData("", 1, "")]
    [InlineData("test", 1, "t")]
    [InlineData(" test ", 1, " ")]
    [InlineData("test", 100, "test")]
    [InlineData(" test", 100, " test")]
    [InlineData("test", 2, "st")]
    public void stringextensions__right_returns_expected(string str, int length, string expected) => str.Right(length).Should().Be(expected);

    // IsValidDate
    // -------------

    [Theory]
    [InlineData("2022-01-01")]
    [InlineData("2022-01-31")]
    [InlineData("2022/01/31")]
    [InlineData("1/31/2022")]
    [InlineData("1/31/22")]
    [InlineData("2022/01/31 01:01:01")]
    [InlineData("10-20-2022 01:01:01")]
    [InlineData("1/2022")]
    public void stringextensions__isvaliddate_returns_true(string str) => str.IsValidDate().Should().BeTrue();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("22-01-31")]
    [InlineData("abc")]
    public void stringextensions__isvaliddate_returns_false(string str) => str.IsValidDate().Should().BeFalse();

    // Split
    // -------------

    [Fact]
    public void stringextensions__split_returns()
    {
        var result = "p,o".Split(",", StringSplitOptions.RemoveEmptyEntries);
        result.Length.Should().Be(2);
        result[0].Should().Be("p");
        result[1].Should().Be("o");
    }

    [Fact]
    public void stringextensions__split_removes_empty_returns()
    {
        var result = "p,".Split(",", StringSplitOptions.RemoveEmptyEntries);
        result.Length.Should().Be(1);
        result[0].Should().Be("p");
    }

    // RemoveInvalidXmlChars
    //

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData(" noop ", " noop ")]
    [InlineData(" remove & ampersand ", " remove  ampersand ")]
    [InlineData(" remove < lessthan ", " remove  lessthan ")]
    [InlineData(" remove > greaterthan ", " remove  greaterthan ")]
    [InlineData(" remove ' singlequote ", " remove  singlequote ")]
    [InlineData(" remove \" doublequote ", " remove  doublequote ")]
    [InlineData(" keeps *()^$4%35@![]{} ", " keeps *()^$4%35@![]{} ")]
    public void stringextensions__remove_invalid_xml_chars(string instr, string expected)
    {
        var result = instr.RemoveInvalidXmlChars();
        result.Should().Be(expected);
    }

    // ReplaceWordSpecialCharacters
    //

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData("tes73837&923470-)098``~@!@$023467}{';\"<>.,/mNM_-=+\"tbegin [“”‘’] testend", "tes73837&923470-)098``~@!@$023467}{';\"<>.,/mNM_-=+\"tbegin [\"\"''] testend")]
    public void stringextensions__replaces_word_special_characters(string instr, string expected)
    {
        var result = instr.ReplaceWordSpecialCharacters();
        result.Should().Be(expected);
    }

    // Replace Tabs
    //

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData("Hey    tab", "Hey tab")]
    [InlineData("   tab-         A", " tab- A")]
    [InlineData("    tab-         A", " tab- A")]
    public void stringextensions__replacetabs(string instr, string expected)
    {
        var result = instr.ReplaceTabs();
        result.Should().Be(expected);
    }

    // Format Phone Number
    //

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", "")]
    [InlineData(null, null)]
    [InlineData("(800) 555 - 5555", "800-555-5555")]
    [InlineData("800-555-5555", "800-555-5555")]
    [InlineData("8005555555", "8005555555")]
    public void stringextensions__formatphonenumber_style_default(string instr, string expected)
    {
        var result = instr.FormatPhoneNumber();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", "")]
    [InlineData(null, null)]
    [InlineData("(800) 555 - 5555", "800-555-5555")]
    [InlineData("800-555-5555", "800-555-5555")]
    [InlineData("8005555555", "8005555555")]
    public void stringextensions__formatphonenumber_style_standard(string instr, string expected)
    {
        var result = instr.FormatPhoneNumber(PhoneFormatStyle.Standard);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("(800) 555 - 5555")]
    [InlineData("800-555-5555")]
    [InlineData("8005555555")]
    public void stringextensions__formatphonenumber_style_none(string instr)
    {
        var result = instr.FormatPhoneNumber(PhoneFormatStyle.None);
        result.Should().Be(instr);
    }
}
