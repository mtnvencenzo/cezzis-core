namespace Cezzi.Applications.Serialization;

using System.Text;
using System.Xml.Serialization;

/// <summary>
/// 
/// </summary>
public class XmlSerializer<T>
{
    #region Constructors & Initialization

    /// <summary>
    /// 
    /// </summary>
    public XmlSerializer()
    {
        this._serializer = new XmlSerializer(typeof(T));
    }

    #endregion

    #region Private Fields

    private readonly XmlSerializer _serializer;

    #endregion

    /// <summary>
    /// Toes the XML.
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <returns></returns>
    public string ToXml(T obj)
    {
        var sb = new StringBuilder();

        using (var writer = new System.IO.StringWriter(sb))
        {
            this._serializer.Serialize(writer, obj);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public T FromXml(string xml)
    {
        T obj = default;

        using (var reader = new System.IO.StringReader(xml))
        {
            obj = (T)this._serializer.Deserialize(reader);
        }

        return obj;
    }
}
