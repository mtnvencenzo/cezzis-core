namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using System.Reflection;
using Xunit;

public class ReflectionExtensions_Tests
{
    [Fact]
    public void reflectionextensinos_isoverride_null_method_false() => (null as MethodInfo).IsOverride().Should().BeFalse();

    [Fact]
    public void reflectionextensinos_isoverride_null_prop_false() => (null as PropertyInfo).IsOverride().Should().BeFalse();

    [Fact]
    public void reflectionextensinos_isoverride_method_false()
    {
        var methodInfo = typeof(SubClass).GetMethod(nameof(SubClass.SomethingElse));
        methodInfo.Should().NotBeNull();

        methodInfo.IsOverride().Should().BeFalse();
    }

    [Fact]
    public void reflectionextensinos_isoverride_method_true()
    {
        var methodInfo = typeof(SubClass).GetMethod(nameof(SubClass.Something));
        methodInfo.Should().NotBeNull();

        methodInfo.IsOverride().Should().BeTrue();
    }

    [Fact]
    public void reflectionextensinos_isoverride_property_false()
    {
        var propinfo = typeof(SubClass).GetProperty(nameof(SubClass.SomethingElseProp));
        propinfo.Should().NotBeNull();

        propinfo.IsOverride().Should().BeFalse();
    }

    [Fact]
    public void reflectionextensinos_isoverride_property_true()
    {
        var propinfo = typeof(SubClass).GetProperty(nameof(SubClass.SomethingProp));
        propinfo.Should().NotBeNull();

        propinfo.IsOverride().Should().BeTrue();
    }
}

public abstract class BaseClass
{
    public virtual void Something()
    {

    }

    public virtual string SomethingProp { get; set; }
}

public class SubClass : BaseClass
{
    public override void Something()
    {

    }

    public virtual void SomethingElse()
    {

    }

    public override string SomethingProp { get; set; }

    public virtual string SomethingElseProp { get; set; }
}
