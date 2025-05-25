namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using System.Xml.Linq;
using Xunit;

public class XmlExtensions_Tests
{
    [Fact]
    public void xmlextensions___addelementifnotnull_when_element_null() => (null as XElement).AddElementIfNotNull("test", "test").Should().BeNull();

    [Fact]
    public void xmlextensions___addelementifnotnull_when_value_null()
    {
        var element = new XElement("test");

        element.AddElementIfNotNull("test2", null).Should().BeSameAs(element);
    }

    [Fact]
    public void xmlextensions___addelementifnotnull_adds()
    {
        var element = new XElement("test");

        element.AddElementIfNotNull("test2", "testvalue").Should().BeSameAs(element);

        element.ToRawString().Should().Be("<test><test2>testvalue</test2></test>");
    }
}
