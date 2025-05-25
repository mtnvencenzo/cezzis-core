namespace Cezzi.Sftp;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// sftp exception
/// </summary>
public class SftpException : Exception
{
    /// <summary>
    /// default constructor
    /// </summary>
    public SftpException() : base() { }

    /// <summary>
    /// overridden constructor with message
    /// </summary>
    /// <param name="message"></param>
    public SftpException(string message) : base(message) { }

    /// <summary>
    /// overridden constructor with message and exception
    /// </summary>
    public SftpException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// overridden constructor with message and exceptions list
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerExceptionList"></param>
    public SftpException(string message, IEnumerable<Exception> innerExceptionList) : base(message)
    {
        foreach (var ex in innerExceptionList)
        {
            this.Data.Add(ex.Message, ex);
        }
    }

    /// <summary>
    /// to string override
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine(base.ToString());

        foreach (var item in this.Data)
        {
            if (item is DictionaryEntry dict)
            {
                sb.AppendLine($"{dict.Key} at {this.Data[dict.Key]}");
            }
            else
            {
                sb.AppendLine($"{item} at {this.Data[item]}");
            }
        }

        return sb.ToString();
    }
}
