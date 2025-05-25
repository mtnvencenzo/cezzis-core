namespace Cezzi.Applications.Tests;

using FluentAssertions;
using System;
using Xunit;

public class AlphabeticalCharConverterTests
{
    [Theory]
    [InlineData('A', 1)]
    [InlineData('B', 2)]
    [InlineData('C', 3)]
    [InlineData('D', 4)]
    [InlineData('E', 5)]
    [InlineData('F', 6)]
    [InlineData('G', 7)]
    [InlineData('H', 8)]
    [InlineData('I', 9)]
    [InlineData('J', 10)]
    [InlineData('K', 11)]
    [InlineData('L', 12)]
    [InlineData('M', 13)]
    [InlineData('N', 14)]
    [InlineData('O', 15)]
    [InlineData('P', 16)]
    [InlineData('Q', 17)]
    [InlineData('R', 18)]
    [InlineData('S', 19)]
    [InlineData('T', 20)]
    [InlineData('U', 21)]
    [InlineData('V', 22)]
    [InlineData('W', 23)]
    [InlineData('X', 24)]
    [InlineData('Y', 25)]
    [InlineData('Z', 26)]
    public void alphacharconverter___converts_chars_to_alphabetical_int_position(char c, int expected)
    {
        var retval = AlphabeticalCharConverter.ToInteger(c);

        retval.Should().Be(expected);
    }

    [Theory]
    [InlineData('A', 1)]
    [InlineData('B', 2)]
    [InlineData('C', 3)]
    [InlineData('D', 4)]
    [InlineData('E', 5)]
    [InlineData('F', 6)]
    [InlineData('G', 7)]
    [InlineData('H', 8)]
    [InlineData('I', 9)]
    [InlineData('J', 10)]
    [InlineData('K', 11)]
    [InlineData('L', 12)]
    [InlineData('M', 13)]
    [InlineData('N', 14)]
    [InlineData('O', 15)]
    [InlineData('P', 16)]
    [InlineData('Q', 17)]
    [InlineData('R', 18)]
    [InlineData('S', 19)]
    [InlineData('T', 20)]
    [InlineData('U', 21)]
    [InlineData('V', 22)]
    [InlineData('W', 23)]
    [InlineData('X', 24)]
    [InlineData('Y', 25)]
    [InlineData('Z', 26)]
    public void alphacharconverter___converts_ints_to_alphabetical_char(char expected, int value)
    {
        var retval = AlphabeticalCharConverter.FromInteger(value);

        retval.Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(27)]
    public void alphacharconverter___throws_when_integer_supplied_not_between_1_and_26(int value)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = AlphabeticalCharConverter.FromInteger(value);
        });

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("value");
        ex.Message.Should().Be("value must be in the range of 1-26 (Parameter 'value')");
    }

    [Theory]
    [InlineData('0')]
    [InlineData('a')]
    public void alphacharconverter___throws_when_char_supplied_not_upper_case_alphabet_char(char value)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = AlphabeticalCharConverter.ToInteger(value);
        });

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("value");
        ex.Message.Should().Be("value must be in the range of A-Z uppercase. (Parameter 'value')");
    }
}
