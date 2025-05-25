namespace Cezzi.Applications.Tests.Compare;

using Cezzi.Applications.Compare;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

public class AscendingAlphabeticComparer_Tests
{
    [Fact]
    public void AscendingAlphabeticComparer___compares_correctly()
    {
        var list = new List<string>
        {
            "def",
            "abc",
            "aab",
            "zed",
        };

        list.Sort(new AscendingAlphabeticComparer());

        list.Should().HaveElementAt(0, "aab");
        list.Should().HaveElementAt(1, "abc");
        list.Should().HaveElementAt(2, "def");
        list.Should().HaveElementAt(3, "zed");
    }
}
