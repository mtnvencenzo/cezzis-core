namespace Cezzi.Azure.ServiceBus;

using System;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TDataObject">The type of the data object.</typeparam>
/// <seealso cref="System.Exception" />
public class ServiceBusDataLossException<TDataObject> : Exception
{
    private readonly TDataObject dataObject;

    /// <summary>Initializes a new instance of the <see cref="ServiceBusDataLossException{TDataObject}"/> class.</summary>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="dataObject">The data object.</param>
    public ServiceBusDataLossException(Exception innerException, TDataObject dataObject)
        : base(innerException?.Message, innerException)
    {
        this.dataObject = dataObject;
    }

    /// <summary>Initializes a new instance of the <see cref="ServiceBusDataLossException{TDataObject}"/> class.</summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="dataObject">The data object.</param>
    public ServiceBusDataLossException(string message, Exception innerException, TDataObject dataObject)
        : base(message, innerException)
    {
        this.dataObject = dataObject;
    }

    /// <summary>Converts to string.</summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        var json = string.Empty;

        try
        {
            json = this.dataObject == null
                ? string.Empty
                : ServiceBusMessageSerializer.SerializeToUtf8String(this.dataObject);
        }
        catch { }

        return $"{base.ToString()}{Environment.NewLine}{Environment.NewLine}::Data Object::{Environment.NewLine}{json}";
    }
}
