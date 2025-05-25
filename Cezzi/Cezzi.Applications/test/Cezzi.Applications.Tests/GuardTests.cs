namespace Cezzi.Applications.Tests;

using Cezzi.Applications;
using FluentAssertions;
using System;
using Xunit;

/// <summary>
/// 
/// </summary>
public class GuardTests
{
    [Fact]
    public void guard___not_null_with_paramname_throws()
    {
        string obj = null;

        var exp = Assert.Throws<ArgumentNullException>(() => Guard.NotNull(obj, nameof(obj)));

        exp.Should().NotBeNull();
        exp.ParamName.Should().Be(nameof(obj));
    }

    [Fact]
    public void guard___not_null_with_no_paramname_throws()
    {
        string obj = null;

        var exp = Assert.Throws<ArgumentNullException>(() => Guard.NotNull(obj));

        exp.Should().NotBeNull();
        exp.ParamName.Should().BeNull();
    }

    [Fact]
    public void guard___not_null_does_not_throw_when_not_null()
    {
        var obj = new object();

        Guard.NotNull(obj);
        Guard.NotNull(obj, nameof(obj));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void guard___not_null_or_whitespace_with_paramname_throws(string inString)
    {
        var exp = Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrWhiteSpace(inString, nameof(inString)));

        exp.Should().NotBeNull();
        exp.ParamName.Should().Be(nameof(inString));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void guard___not_null_or_whitespace_with_no_paramname_throws(string inString)
    {
        var exp = Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrWhiteSpace(inString, paramName: null));

        exp.Should().NotBeNull();
        exp.ParamName.Should().BeNull();
    }

    [Fact]
    public void guard___not_null_or_whitespace_does_not_throw_when_not_null()
    {
        var str = "a";

        Guard.NotNullOrWhiteSpace(str);
        Guard.NotNullOrWhiteSpace(str, nameof(str));
    }

    [Fact]
    public void guard___not_enum_member_with_paramname_throws()
    {
        var enumValue = TestEnum.Member1;

        var exp = Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotEnumMember(
            value: enumValue,
            member: TestEnum.Member1,
            paramName: nameof(enumValue)));

        exp.Should().NotBeNull();
        exp.ParamName.Should().Be(nameof(enumValue));
    }

    [Fact]
    public void guard___not_enum_member_with_no_paramname_throws()
    {
        var enumValue = TestEnum.Member1;

        var exp = Assert.Throws<ArgumentOutOfRangeException>(() => Guard.NotEnumMember(
            value: enumValue,
            member: TestEnum.Member1,
            paramName: null));

        exp.Should().NotBeNull();
        exp.ParamName.Should().BeNull();
    }

    [Fact]
    public void guard___not_enum_member_does_not_throw_when_not_enum_member()
    {
        var enumValue = TestEnum.Member1;

        Guard.NotEnumMember(
           value: enumValue,
           member: TestEnum.None,
           paramName: nameof(enumValue));

        Guard.NotEnumMember(
           value: enumValue,
           member: TestEnum.None,
           paramName: null);
    }

    [Fact]
    public void guard___not_default_using_default_keyword_with_paramname_throws()
    {
        TestClass testClass = default;

        var exp = Assert.Throws<ArgumentException>(() => Guard.NotDefault(
            value: testClass,
            paramName: nameof(testClass)));

        exp.Should().NotBeNull();
        exp.ParamName.Should().Be(nameof(testClass));
    }

    [Fact]
    public void guard___not_default_using_null_with_paramname_throws()
    {
        TestClass testClass = null;

        var exp = Assert.Throws<ArgumentException>(() => Guard.NotDefault(
            value: testClass,
            paramName: nameof(testClass)));

        exp.Should().NotBeNull();
        exp.ParamName.Should().Be(nameof(testClass));
    }

    [Fact]
    public void guard___not_default_with_no_paramname_throws()
    {
        TestClass testClass = null;

        var exp = Assert.Throws<ArgumentException>(() => Guard.NotDefault(
            value: testClass,
            paramName: null));

        exp.Should().NotBeNull();
        exp.ParamName.Should().BeNull();
    }

    [Fact]
    public void guard___not_default_does_not_throw_when_not_default()
    {
        var testClass = new TestClass();

        Guard.NotDefault(value: testClass, paramName: nameof(testClass));
        Guard.NotDefault(value: testClass, paramName: null);
    }

    [Fact]
    public void guard___not_default_for_value_type_throws()
    {
        TestEnum testEnum = default;

        var exp = Assert.Throws<ArgumentException>(() => Guard.NotDefault(
            value: testEnum,
            paramName: nameof(testEnum)));

        exp.Should().NotBeNull();
        exp.ParamName.Should().Be(nameof(testEnum));
    }

    [Fact]
    public void guard___one_or_less_with_paramname_throws()
    {
        var strings = new[] { "1", "2" };

        var exp = Assert.Throws<ArgumentOutOfRangeException>(() => Guard.OneOrLess(
            list: strings,
            paramName: nameof(strings)));

        exp.Should().NotBeNull();
        exp.ParamName.Should().Be(nameof(strings));
    }

    [Fact]
    public void guard___one_or_less_with_no_paramname_throws()
    {
        var strings = new[] { "1", "2" };

        var exp = Assert.Throws<ArgumentOutOfRangeException>(() => Guard.OneOrLess(
            list: strings,
            paramName: null));

        exp.Should().NotBeNull();
        exp.ParamName.Should().BeNull();
    }

    [Fact]
    public void guard___one_or_less_does_not_throw_for_null_list()
    {
        string[] strings = null;

        Guard.OneOrLess(list: strings, paramName: nameof(strings));
        Guard.OneOrLess(list: strings, paramName: null);
    }

    [Fact]
    public void guard___one_or_less_does_not_throw_for_empty_list()
    {
        var strings = Array.Empty<string>();

        Guard.OneOrLess(list: strings, paramName: nameof(strings));
        Guard.OneOrLess(list: strings, paramName: null);
    }

    [Fact]
    public void guard___one_or_less_does_not_throw_for_one_item_in_list()
    {
        var strings = new string[] { "1" };

        Guard.OneOrLess(list: strings, paramName: nameof(strings));
        Guard.OneOrLess(list: strings, paramName: null);
    }

    [Fact]
    public void guard___not_empty_with_empty_list_and_paramname_throws()
    {
        var strings = Array.Empty<string>();

        var exp = Assert.Throws<ArgumentException>(() => Guard.NotEmpty(
            list: strings,
            paramName: nameof(strings)));

        exp.Should().NotBeNull();
        exp.ParamName.Should().Be(nameof(strings));
    }

    [Fact]
    public void guard___not_empty_with_null_list_and_paramname_throws()
    {
        string[] strings = null;

        var exp = Assert.Throws<ArgumentException>(() => Guard.NotEmpty(
            list: strings,
            paramName: nameof(strings)));

        exp.Should().NotBeNull();
        exp.ParamName.Should().Be(nameof(strings));
    }

    [Fact]
    public void guard___not_empty_with_null_list_and_no_paramname_throws()
    {
        string[] strings = null;

        var exp = Assert.Throws<ArgumentException>(() => Guard.NotEmpty(
            list: strings,
            paramName: null));

        exp.Should().NotBeNull();
        exp.ParamName.Should().BeNull();
    }

    [Fact]
    public void guard___of_type_throws_for_different_type()
    {
        var ex = Assert.Throws<ArgumentException>(
            paramName: "value",
            testCode: () => Guard.OfType<TestClass>(TestEnum.Member1, "value"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___of_type_throws_for_null_value()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            paramName: "value",
            testCode: () => Guard.OfType<TestClass>(null, "value"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___does_not_throw_for_same_type()
    {
        var test = new TestClass();

        Guard.OfType<TestClass>(test);
    }

    [Fact]
    public void guard___positive_does_not_throw_for_positive_short() => Guard.Positive((short)1);

    [Fact]
    public void guard___positive_throw_for_default_short()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive(default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_throw_for_negative_short()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive((short)-1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_does_not_throw_for_positive_int() => Guard.Positive(1);

    [Fact]
    public void guard___positive_throw_for_default_int()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive((int)default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_throw_for_negative_int()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive(-1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_does_not_throw_for_positive_long() => Guard.Positive((long)1);

    [Fact]
    public void guard___positive_throw_for_default_long()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive((long)default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_throw_for_negative_long()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive((long)-1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_does_not_throw_for_positive_float() => Guard.Positive((float).1);

    [Fact]
    public void guard___positive_throw_for_default_float()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive((float)default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_throw_for_negative_float()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive((float)-.1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_does_not_throw_for_positive_double() => Guard.Positive((double).1);

    [Fact]
    public void guard___positive_throw_for_default_double()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive((double)default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_throw_for_negative_double()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive((double)-.1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_does_not_throw_for_positive_decimal() => Guard.Positive((decimal).1);

    [Fact]
    public void guard___positive_throw_for_default_decimal()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive((decimal)default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___positive_throw_for_negative_decimal()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Positive((decimal)-.1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_does_not_throw_for_negative_short() => Guard.Negative((short)-1);

    [Fact]
    public void guard___negative_throw_for_default_short()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative(default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_throw_for_positive_short()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative((short)1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_does_not_throw_for_negative_int() => Guard.Negative(-1);

    [Fact]
    public void guard___negative_throw_for_default_int()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative((int)default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_throw_for_positive_int()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative(1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_does_not_throw_for_negative_long() => Guard.Negative((long)-1);

    [Fact]
    public void guard___negative_throw_for_default_long()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative((long)default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_throw_for_positive_long()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative((long)1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_does_not_throw_for_negative_float() => Guard.Negative((float)-.1);

    [Fact]
    public void guard___negative_throw_for_default_float()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative((float)default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_throw_for_positive_float()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative((float).1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_does_not_throw_for_negative_decimal() => Guard.Negative((decimal)-.1);

    [Fact]
    public void guard___negative_throw_for_default_decimal()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative((decimal)default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_throw_for_positive_decimal()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative((decimal).1, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_does_not_throw_for_negative_double() => Guard.Negative((double)-.1);

    [Fact]
    public void guard___negative_throw_for_default_double()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative((double)default, "myal"));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void guard___negative_throw_for_positive_double()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            paramName: "myal",
            testCode: () => Guard.Negative((double).1, "myal"));

        ex.Should().NotBeNull();
    }

    private enum TestEnum
    {
        /// <summary>The none</summary>
        None = 0,

        /// <summary>The member1</summary>
        Member1 = 1
    }

    private class TestClass { }
}
