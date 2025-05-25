namespace Cezzi.Data;

using System;
using System.Data;

//-----------------------------------------------------------------------
// <copyright file="SafeDataReader.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>This is a DataReader that 'fixes' any null values before</summary>
//-----------------------------------------------------------------------
/// <summary>
/// This is a DataReader that 'fixes' any null values before
/// they are returned to our business code.
/// </summary>
/// <remarks>
/// Initializes the SafeDataReader object to use data from
/// the provided DataReader object.
/// </remarks>
/// <param name="dataReader">The source DataReader object containing the data.</param>
public class SafeDataReader(IDataReader dataReader) : IDataReader
{
    private bool disposedValue;

    /// <summary>
    /// Get a reference to the underlying data reader
    /// object that actually contains the data from
    /// the data source.
    /// </summary>
    protected IDataReader DataReader { get; } = dataReader;

    /// <summary>
    /// Gets a string value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns empty string for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public string GetString(string name) => this.GetString(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a string value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns empty string for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual string GetString(int i) => this.DataReader.IsDBNull(i) ? string.Empty : this.DataReader.GetString(i);

    /// <summary>
    /// Gets a value of type <see cref="object" /> from the datareader.
    /// </summary>
    /// <param name="name">Name of the column containing the value.</param>
    public object GetValue(string name) => this.GetValue(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a value of type <see cref="object" /> from the datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual object GetValue(int i) => this.DataReader.IsDBNull(i) ? null : this.DataReader.GetValue(i);

    /// <summary>
    /// Gets an integer from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public int GetInt32(string name) => this.GetInt32(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets an integer from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual int GetInt32(int i) => this.DataReader.IsDBNull(i) ? 0 : this.DataReader.GetInt32(i);

    /// <summary>
    /// Gets a double from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public double GetDouble(string name) => this.GetDouble(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a double from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual double GetDouble(int i) => this.DataReader.IsDBNull(i) ? 0 : this.DataReader.GetDouble(i);

    /// <summary>
    /// Gets a Guid value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns Guid.Empty for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public Guid GetGuid(string name) => this.GetGuid(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a Guid value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns Guid.Empty for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual Guid GetGuid(int i) => this.DataReader.IsDBNull(i) ? Guid.Empty : this.DataReader.GetGuid(i);

    /// <summary>
    /// Reads the next row of data from the datareader.
    /// </summary>
    public bool Read() => this.DataReader.Read();

    /// <summary>
    /// Moves to the next result set in the datareader.
    /// </summary>
    public bool NextResult() => this.DataReader.NextResult();

    /// <summary>
    /// Closes the datareader.
    /// </summary>
    public void Close() => this.DataReader.Close();

    /// <summary>
    /// Returns the depth property value from the datareader.
    /// </summary>
    public int Depth => this.DataReader.Depth;

    /// <summary>
    /// Returns the FieldCount property from the datareader.
    /// </summary>
    public int FieldCount => this.DataReader.FieldCount;

    /// <summary>
    /// Gets a boolean value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns <see langword="false" /> for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public bool GetBoolean(string name) => this.GetBoolean(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a boolean value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns <see langword="false" /> for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual bool GetBoolean(int i) => !this.DataReader.IsDBNull(i) && this.DataReader.GetBoolean(i);

    /// <summary>
    /// Gets a byte value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public byte GetByte(string name) => this.GetByte(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a byte value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual byte GetByte(int i) => this.DataReader.IsDBNull(i) ? (byte)0 : this.DataReader.GetByte(i);

    /// <summary>
    /// Invokes the GetBytes method of the underlying datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="buffer">Array containing the data.</param>
    /// <param name="bufferOffset">Offset position within the buffer.</param>
    /// <param name="fieldOffset">Offset position within the field.</param>
    /// <param name="length">Length of data to read.</param>
    public long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferOffset, int length) => this.GetBytes(this.DataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);

    /// <summary>
    /// Invokes the GetBytes method of the underlying datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    /// <param name="buffer">Array containing the data.</param>
    /// <param name="bufferOffset">Offset position within the buffer.</param>
    /// <param name="fieldOffset">Offset position within the field.</param>
    /// <param name="length">Length of data to read.</param>
    public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length) => this.DataReader.IsDBNull(i) ? 0 : this.DataReader.GetBytes(i, fieldOffset, buffer, bufferOffset, length);

    /// <summary>
    /// Gets a char value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns Char.MinValue for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public char GetChar(string name) => this.GetChar(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a char value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns Char.MinValue for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual char GetChar(int i)
    {
        if (this.DataReader.IsDBNull(i))
        {
            return char.MinValue;
        }
        else
        {
            var myChar = new char[1];
            this.DataReader.GetChars(i, 0, myChar, 0, 1);
            return myChar[0];
        }
    }

    /// <summary>
    /// Invokes the GetChars method of the underlying datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    /// <param name="buffer">Array containing the data.</param>
    /// <param name="bufferOffset">Offset position within the buffer.</param>
    /// <param name="fieldOffset">Offset position within the field.</param>
    /// <param name="length">Length of data to read.</param>
    public long GetChars(string name, long fieldOffset, char[] buffer, int bufferOffset, int length) => this.GetChars(this.DataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);

    /// <summary>
    /// Invokes the GetChars method of the underlying datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    /// <param name="buffer">Array containing the data.</param>
    /// <param name="bufferOffset">Offset position within the buffer.</param>
    /// <param name="fieldOffset">Offset position within the field.</param>
    /// <param name="length">Length of data to read.</param>
    public virtual long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length) => this.DataReader.IsDBNull(i) ? 0 : this.DataReader.GetChars(i, fieldOffset, buffer, bufferOffset, length);

    /// <summary>
    /// Invokes the GetData method of the underlying datareader.
    /// </summary>
    /// <param name="name">Name of the column containing the value.</param>
    public IDataReader GetData(string name) => this.GetData(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Invokes the GetData method of the underlying datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual IDataReader GetData(int i) => this.DataReader.GetData(i);

    /// <summary>
    /// Invokes the GetDataTypeName method of the underlying datareader.
    /// </summary>
    /// <param name="name">Name of the column containing the value.</param>
    public string GetDataTypeName(string name) => this.GetDataTypeName(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Invokes the GetDataTypeName method of the underlying datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual string GetDataTypeName(int i) => this.DataReader.GetDataTypeName(i);

    /// <summary>
    /// Gets a date value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns DateTime.MinValue for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public virtual DateTime GetDateTime(string name) => this.GetDateTime(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a date value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns DateTime.MinValue for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual DateTime GetDateTime(int i) => this.DataReader.IsDBNull(i) ? DateTime.MinValue : this.DataReader.GetDateTime(i);

    /// <summary>Gets the date time offset.</summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public virtual DateTimeOffset GetDateTimeOffset(string name) => this.GetDateTimeOffset(this.DataReader.GetOrdinal(name));

    /// <summary>Gets the date time offset.</summary>
    /// <param name="i">The i.</param>
    /// <returns></returns>
    public virtual DateTimeOffset GetDateTimeOffset(int i) => this.DataReader.IsDBNull(i) ? DateTimeOffset.MinValue : (DateTimeOffset)this.DataReader[i];

    /// <summary>
    /// Gets a decimal value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public decimal GetDecimal(string name) => this.GetDecimal(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a decimal value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual decimal GetDecimal(int i) => this.DataReader.IsDBNull(i) ? 0 : this.DataReader.GetDecimal(i);

    /// <summary>
    /// Invokes the GetFieldType method of the underlying datareader.
    /// </summary>
    /// <param name="name">Name of the column containing the value.</param>
    public Type GetFieldType(string name) => this.GetFieldType(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Invokes the GetFieldType method of the underlying datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual Type GetFieldType(int i) => this.DataReader.GetFieldType(i);

    /// <summary>
    /// Gets a Single value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public float GetFloat(string name) => this.GetFloat(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a Single value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual float GetFloat(int i) => this.DataReader.IsDBNull(i) ? 0 : this.DataReader.GetFloat(i);

    /// <summary>
    /// Gets a Short value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public short GetInt16(string name) => this.GetInt16(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a Short value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual short GetInt16(int i) => this.DataReader.IsDBNull(i) ? (short)0 : this.DataReader.GetInt16(i);

    /// <summary>
    /// Gets a Long value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public long GetInt64(string name) => this.GetInt64(this.DataReader.GetOrdinal(name));

    /// <summary>
    /// Gets a Long value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns 0 for null.
    /// </remarks>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual long GetInt64(int i) => this.DataReader.IsDBNull(i) ? 0 : this.DataReader.GetInt64(i);

    /// <summary>
    /// Invokes the GetName method of the underlying datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual string GetName(int i) => this.DataReader.GetName(i);

    /// <summary>
    /// Gets an ordinal value from the datareader.
    /// </summary>
    /// <param name="name">Name of the column containing the value.</param>
    public int GetOrdinal(string name) => this.DataReader.GetOrdinal(name);

    /// <summary>
    /// Invokes the GetSchemaTable method of the underlying datareader.
    /// </summary>
    public DataTable GetSchemaTable() => this.DataReader.GetSchemaTable();

    /// <summary>
    /// Invokes the GetValues method of the underlying datareader.
    /// </summary>
    /// <param name="values">An array of System.Object to
    /// copy the values into.</param>
    public int GetValues(object[] values) => this.DataReader.GetValues(values);

    /// <summary>
    /// Returns the IsClosed property value from the datareader.
    /// </summary>
    public bool IsClosed => this.DataReader.IsClosed;

    /// <summary>
    /// Invokes the IsDBNull method of the underlying datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual bool IsDBNull(int i) => this.DataReader.IsDBNull(i);

    /// <summary>
    /// Invokes the IsDBNull method of the underlying datareader.
    /// </summary>
    /// <param name="name">Name of the column containing the value.</param>
    public virtual bool IsDBNull(string name)
    {
        var index = this.GetOrdinal(name);
        return this.IsDBNull(index);
    }

    /// <summary>
    /// Returns a value from the datareader.
    /// </summary>
    /// <param name="name">Name of the column containing the value.</param>
    public object this[string name]
    {
        get
        {
            var val = this.DataReader[name];
            return DBNull.Value.Equals(val) ? null : val;
        }
    }

    /// <summary>
    /// Returns a value from the datareader.
    /// </summary>
    /// <param name="i">Ordinal column position of the value.</param>
    public virtual object this[int i] => this.DataReader.IsDBNull(i) ? null : this.DataReader[i];

    /// <summary>
    /// Returns the RecordsAffected property value from the underlying datareader.
    /// </summary>
    public int RecordsAffected => this.DataReader.RecordsAffected;

    /// <summary>
    /// Disposes the object.
    /// </summary>
    /// <param name="disposing">True if called by
    /// the public Dispose method.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                // free unmanaged resources when explicitly called
                this.DataReader.Dispose();
            }
        }

        this.disposedValue = true;
    }

    /// <summary>
    /// Disposes the object.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Object finalizer.
    /// </summary>
    ~SafeDataReader()
    {
        this.Dispose(false);
    }
}
