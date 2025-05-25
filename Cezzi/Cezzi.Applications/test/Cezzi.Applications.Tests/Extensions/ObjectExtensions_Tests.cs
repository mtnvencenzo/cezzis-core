namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using System;
using Xunit;

public class ObjectExtensions_Tests
{
    [Fact]
    public void objectextensions___project_retuns_default_struct_for_null_func()
    {
        var i = 9;

        var projection = i.Project<int, long>(projection: null);

        projection.Should().Be(default);
    }

    [Fact]
    public void objectextensions___project_retuns_default_object_for_null_func()
    {
        var i = 9;

        var projection = i.Project<int, DateTime>(projection: null);

        projection.Should().Be(default);
    }

    [Fact]
    public void objectextensions___project_retuns_default_class_for_null_func()
    {
        var i = 9;

        var projection = i.Project<int, EventArgs>(projection: null);

        projection.Should().Be(null);
    }

    [Fact]
    public void objectextensions___project_retuns_projection()
    {
        var i = 9;

        var projection = i.Project<int, TimeSpan>((i) => TimeSpan.FromSeconds(i));

        projection.Seconds.Should().Be(9);
    }
}
