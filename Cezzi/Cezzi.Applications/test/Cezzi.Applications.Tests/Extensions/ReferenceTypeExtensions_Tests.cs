namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using System;
using Xunit;

public class ReferenceTypeExtensions_Tests
{
    [Fact]
    public void referencetypeextensions___NullIf_throws_if_predicate_null()
    {
        var strings = new[] { "test" };

        var ex = Assert.Throws<ArgumentNullException>(
            paramName: "predicate",
            testCode: () => strings.NullIf(null));

        ex.Should().NotBeNull();
    }

    [Fact]
    public void referencetypeextensions___NullIf_returns_null_if_predicate_true()
    {
        var strings = new[] { "test" };

        var result = strings.NullIf((s) => true);

        result.Should().BeNull();
    }

    [Fact]
    public void referencetypeextensions___NullIf_returns_obj_if_predicate_false()
    {
        var strings = new[] { "test" };

        var result = strings.NullIf((s) => false);

        result.Should().BeSameAs(strings);
    }
}
