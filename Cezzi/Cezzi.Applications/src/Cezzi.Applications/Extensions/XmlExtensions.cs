namespace Cezzi.Applications.Extensions;

using System.IO;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// 
/// </summary>
public static class XmlExtensions
{
    /// <summary>Adds the element if not null and returns the parent that the element was added to. (element)</summary>
    /// <param name="element">The element.</param>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static XElement AddElementIfNotNull(this XElement element, string name, object value)
    {
        if (element == null)
        {
            return element;
        }

        if (value == null)
        {
            return element;
        }

        element.Add(new XElement(name, value));
        return element;
    }

    /// <summary>To the raw string.</summary>
    /// <param name="element">The element.</param>
    /// <returns></returns>
    public static string ToRawString(this XElement element)
    {
        using var str = new StringWriter();
        using var writer = XmlWriter.Create(str, new XmlWriterSettings { OmitXmlDeclaration = true });

        element.WriteTo(writer);
        writer.Flush();
        return str.ToString();
    }
}
