namespace Cezzi.Data.Extensions;

using System;
using System.Data;
using System.Xml.Linq;

/// <summary>
/// 
/// </summary>
public static class IDbCommandExtensions
{
    /// <summary>Adds the output parameter.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static TCommand AddOutputParameter<TCommand>(this TCommand cmd, string paramName, DbType type) where TCommand : IDbCommand
    {
        var param = cmd.CreateParameter();
        param.DbType = type;
        param.ParameterName = paramName;
        param.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Adds the output parameter.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="size">The size.</param>
    /// <returns></returns>
    public static TCommand AddOutputParameter<TCommand>(this TCommand cmd, string paramName, DbType type, int size) where TCommand : IDbCommand
    {
        var param = cmd.CreateParameter();
        param.DbType = type;
        param.ParameterName = paramName;
        param.Direction = ParameterDirection.Output;
        param.Size = size;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Adds the return parameter.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static TCommand AddReturnParameter<TCommand>(this TCommand cmd, string paramName, DbType type) where TCommand : IDbCommand
    {
        var param = cmd.CreateParameter();
        param.DbType = type;
        param.ParameterName = paramName;
        param.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Adds the XML parameter.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="xmlString">The XML string.</param>
    /// <returns></returns>
    public static TCommand AddXmlParameter<TCommand>(this TCommand cmd, string paramName, string xmlString) where TCommand : IDbCommand
    {
        var param = cmd.CreateParameter();
        param.DbType = DbType.Xml;
        param.ParameterName = paramName;
        param.Value = xmlString;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Adds the XML parameter.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="xml">The XML.</param>
    /// <returns></returns>
    public static TCommand AddXmlParameter<TCommand>(this TCommand cmd, string paramName, XElement xml) where TCommand : IDbCommand
    {
        var param = cmd.CreateParameter();
        param.DbType = DbType.Xml;
        param.ParameterName = paramName;
        param.Value = xml.ToString();
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Adds the parameter.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static TCommand AddParameter<TCommand>(this TCommand cmd, string paramName, DbType type, object value) where TCommand : IDbCommand
    {
        var param = cmd.CreateParameter();
        param.DbType = type;
        param.ParameterName = paramName;
        param.Value = value;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Adds the parameter.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="size">The size.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static TCommand AddParameter<TCommand>(this TCommand cmd, string paramName, DbType type, int size, object value) where TCommand : IDbCommand
    {
        var param = cmd.CreateParameter();
        param.DbType = type;
        param.ParameterName = paramName;
        param.Value = value;
        param.Size = size;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Adds the parameter if.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="value">The value.</param>
    /// <param name="condition">The condition.</param>
    /// <returns></returns>
    public static TCommand AddParameterIf<TCommand>(this TCommand cmd, string paramName, DbType type, object value, Func<bool> condition) where TCommand : IDbCommand
    {
        if (condition())
        {
            var param = cmd.CreateParameter();
            param.DbType = type;
            param.ParameterName = paramName;
            param.Value = value;
            cmd.Parameters.Add(param);
        }

        return cmd;
    }

    /// <summary>Adds the parameter if.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="valueResolver">The value resolver.</param>
    /// <param name="condition">The condition.</param>
    /// <returns></returns>
    public static TCommand AddParameterIf<TCommand>(this TCommand cmd, string paramName, DbType type, Func<object> valueResolver, Func<bool> condition) where TCommand : IDbCommand
    {
        if (condition())
        {
            var param = cmd.CreateParameter();
            param.DbType = type;
            param.ParameterName = paramName;
            param.Value = valueResolver();
            cmd.Parameters.Add(param);
        }

        return cmd;
    }

    /// <summary>Adds the parameter if.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="size">The size.</param>
    /// <param name="value">The value.</param>
    /// <param name="condition">The condition.</param>
    /// <returns></returns>
    public static TCommand AddParameterIf<TCommand>(this TCommand cmd, string paramName, DbType type, int size, object value, Func<bool> condition) where TCommand : IDbCommand
    {
        if (condition())
        {
            var param = cmd.CreateParameter();
            param.DbType = type;
            param.Size = size;
            param.ParameterName = paramName;
            param.Value = value;
            cmd.Parameters.Add(param);
        }

        return cmd;
    }

    /// <summary>Adds the parameter if.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="size">The size.</param>
    /// <param name="valueResolver">The value resolver.</param>
    /// <param name="condition">The condition.</param>
    /// <returns></returns>
    public static TCommand AddParameterIf<TCommand>(this TCommand cmd, string paramName, DbType type, int size, Func<object> valueResolver, Func<bool> condition) where TCommand : IDbCommand
    {
        if (condition())
        {
            var param = cmd.CreateParameter();
            param.DbType = type;
            param.Size = size;
            param.ParameterName = paramName;
            param.Value = valueResolver();
            cmd.Parameters.Add(param);
        }

        return cmd;
    }

    /// <summary>Adds the structured parameter.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static TCommand AddStructuredParameter<TCommand>(this TCommand cmd, string paramName, DataTable value) where TCommand : IDbCommand
    {
        // From SqlDbType enumeration
        /*
            //
            // Summary:
            //     A special data type for specifying structured data contained in table-valued
            //     parameters.
            Structured = 30,
         */

        var param = cmd.CreateParameter();
        param.GetType().GetProperty("SqlDbType")?.SetValue(param, 30);
        param.ParameterName = paramName;
        param.Value = value;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Sets the stored procedure.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <param name="commandText">The command text.</param>
    /// <returns></returns>
    public static TCommand SetStoredProcedure<TCommand>(this TCommand cmd, string commandText) where TCommand : IDbCommand
    {
        cmd.CommandText = commandText;
        cmd.CommandType = CommandType.StoredProcedure;
        return cmd;
    }

    /// <summary>Clones the database command.</summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="cmd">The command.</param>
    /// <returns></returns>
    public static TCommand CloneDbCommand<TCommand>(this TCommand cmd) where TCommand : IDbCommand
    {
        if (cmd == null)
        {
            return default;
        }

        var newCmd = cmd.Connection.CreateCommand();
        newCmd.CommandText = cmd.CommandText;
        newCmd.CommandType = cmd.CommandType;

        foreach (IDataParameter parameter in cmd.Parameters)
        {
            var newParam = cmd.CreateParameter();
            newParam.ParameterName = parameter.ParameterName;
            newParam.Direction = parameter.Direction;
            newParam.Value = parameter.Value;
            newCmd.Parameters.Add(newParam);
        }

        return (TCommand)newCmd;
    }
}
