namespace Cezzi.Azure.ServiceBus;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// 
/// </summary>
public class ServiceBusMessageSerializer
{
    private readonly static JsonSerializerOptions jsonSerializerOptions;

    /// <summary>Initializes the <see cref="ServiceBusMessageSerializer"/> class.</summary>
    static ServiceBusMessageSerializer()
    {
        jsonSerializerOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(
            namingPolicy: JsonNamingPolicy.CamelCase,
            allowIntegerValues: true)
        );
    }

    /// <summary>Serializes to UTF8 bytes.</summary>
    /// <typeparam name="TMessageObj">The type of the message object.</typeparam>
    /// <param name="messageObj">The message object.</param>
    /// <returns></returns>
    public static byte[] SerializeToUtf8Bytes<TMessageObj>(TMessageObj messageObj) => JsonSerializer.SerializeToUtf8Bytes(messageObj, messageObj.GetType(), jsonSerializerOptions);

    /// <summary>Serializes to UTF8 string.</summary>
    /// <typeparam name="TMessageObj">The type of the message object.</typeparam>
    /// <param name="messageObj">The message object.</param>
    /// <returns></returns>
    public static string SerializeToUtf8String<TMessageObj>(TMessageObj messageObj) => JsonSerializer.Serialize(messageObj, messageObj.GetType(), jsonSerializerOptions);

    /// <summary>Froms the json string.</summary>
    /// <typeparam name="TMessageObj">The type of the message object.</typeparam>
    /// <param name="json">The json.</param>
    /// <returns></returns>
    public static TMessageObj FromJsonString<TMessageObj>(string json) => JsonSerializer.Deserialize<TMessageObj>(json, jsonSerializerOptions);
}
