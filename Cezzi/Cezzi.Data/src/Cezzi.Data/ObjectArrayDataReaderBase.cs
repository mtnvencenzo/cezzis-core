namespace Cezzi.Data;

using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// 
/// </summary>
/// <seealso cref="System.Data.IDataReader" />
public abstract class ObjectArrayDataReaderBase : IDataReader
{
    private IList<(IList<string> columnNames, IList<object[]> datarows)> results;
    private (IList<string> columnNames, IList<object[]> datarows) currentResult;
    private object[] currentDataRow;
    private int currentDataIndex;
    private int currentResultIndex;

    /// <summary>Adds the result internal.</summary>
    /// <param name="columnNames">The column names.</param>
    /// <param name="datarows">The datarows.</param>
    /// <returns></returns>
    protected virtual ObjectArrayDataReaderBase AddResultInternal(
        IList<string> columnNames,
        IList<object[]> datarows)
    {
        if (this.results == null)
        {
            this.IsClosed = false;
            this.results = new List<(IList<string>, IList<object[]>)>
            {
                (columnNames, datarows)
            };

            this.currentDataIndex = -1;
            this.currentResultIndex = 0;
            this.currentResult = this.results[this.currentResultIndex];
        }
        else
        {
            this.results.Add((columnNames, datarows));
        }

        return this;
    }

    /// <summary>Gets the <see cref="object"/> with the specified i.</summary>
    /// <value>The <see cref="object"/>.</value>
    /// <param name="i">The i.</param>
    /// <returns></returns>
    public object this[int i] => this.currentDataRow[i];

    /// <summary>Gets the <see cref="object"/> with the specified name.</summary>
    /// <value>The <see cref="object"/>.</value>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public object this[string name] => this.currentDataRow[this.currentResult.columnNames.IndexOf(name)];

    /// <summary>Gets a value indicating the depth of nesting for the current row.
    /// </summary>
    public virtual int Depth => this.currentDataRow?.Length ?? -1;

    /// <summary>Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
    /// </summary>
    public virtual int RecordsAffected => 0;

    /// <summary>Gets the number of columns in the current row.
    /// </summary>
    public virtual int FieldCount => this.currentResult.columnNames.Count;

    /// <summary>Gets a value indicating whether the data reader is closed.
    /// </summary>
    public virtual bool IsClosed { get; private set; }

    /// <summary>Gets the value of the specified column as a Boolean.</summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>The value of the column.</returns>
    public virtual bool GetBoolean(int i) => (bool)this.currentDataRow[i];

    /// <summary>Gets the 8-bit unsigned integer value of the specified column.</summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>The 8-bit unsigned integer value of the specified column.</returns>
    public virtual byte GetByte(int i) => (byte)this.currentDataRow[i];

    /// <summary>Reads a stream of bytes from the specified column offset into the buffer as an array, starting at the given buffer offset.</summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
    /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
    /// <param name="bufferoffset">The index for <paramref name="buffer" /> to start the read operation.</param>
    /// <param name="length">The number of bytes to read.</param>
    /// <returns>The actual number of bytes read.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();

    /// <summary>Gets the character value of the specified column.</summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <returns>The character value of the specified column.</returns>
    public virtual char GetChar(int i) => (char)this.currentDataRow[i];

    /// <summary>Reads a stream of characters from the specified column offset into the buffer as an array, starting at the given buffer offset.</summary>
    /// <param name="i">The zero-based column ordinal.</param>
    /// <param name="fieldoffset">The index within the row from which to start the read operation.</param>
    /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
    /// <param name="bufferoffset">The index for <paramref name="buffer" /> to start the read operation.</param>
    /// <param name="length">The number of bytes to read.</param>
    /// <returns>The actual number of characters read.</returns>
    public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
        var value = this[i];

        var strval = value != null
            ? Convert.ToString(value)
            : null;

        if (!string.IsNullOrWhiteSpace(strval))
        {
            buffer[0] = strval.ToCharArray()[0];
        }

        return 1;
    }

    /// <summary>Returns an <see cref="T:System.Data.IDataReader" /> for the specified column ordinal.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The <see cref="T:System.Data.IDataReader" /> for the specified column ordinal.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual IDataReader GetData(int i) => throw new NotImplementedException();

    /// <summary>Gets the data type information for the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The data type information for the specified field.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual string GetDataTypeName(int i) => throw new NotImplementedException();

    /// <summary>Gets the date and time data value of the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The date and time data value of the specified field.</returns>
    public virtual DateTime GetDateTime(int i) => (DateTime)this.currentDataRow[i];

    /// <summary>Gets the fixed-position numeric value of the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The fixed-position numeric value of the specified field.</returns>
    public virtual decimal GetDecimal(int i) => (decimal)this.currentDataRow[i];

    /// <summary>Gets the double-precision floating point number of the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The double-precision floating point number of the specified field.</returns>
    public virtual double GetDouble(int i) => (double)this.currentDataRow[i];

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> information corresponding to the type of <see cref="T:System.Object" /> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)" />.
    /// </summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>
    /// The <see cref="T:System.Type" /> information corresponding to the type of <see cref="T:System.Object" /> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)" />.
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual Type GetFieldType(int i) => throw new NotImplementedException();

    /// <summary>Gets the single-precision floating point number of the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The single-precision floating point number of the specified field.</returns>
    public virtual float GetFloat(int i) => (float)this.currentDataRow[i];

    /// <summary>Returns the GUID value of the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The GUID value of the specified field.</returns>
    public virtual Guid GetGuid(int i) => (Guid)this.currentDataRow[i];

    /// <summary>Gets the 16-bit signed integer value of the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The 16-bit signed integer value of the specified field.</returns>
    public virtual short GetInt16(int i) => (short)this.currentDataRow[i];

    /// <summary>Gets the 32-bit signed integer value of the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The 32-bit signed integer value of the specified field.</returns>
    public virtual int GetInt32(int i) => (int)this.currentDataRow[i];

    /// <summary>Gets the 64-bit signed integer value of the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The 64-bit signed integer value of the specified field.</returns>
    public virtual long GetInt64(int i) => (long)this.currentDataRow[i];

    /// <summary>Gets the name for the field to find.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
    public virtual string GetName(int i) => this.currentResult.columnNames[i];

    /// <summary>Return the index of the named field.</summary>
    /// <param name="name">The name of the field to find.</param>
    /// <returns>The index of the named field.</returns>
    public virtual int GetOrdinal(string name) => this.currentResult.columnNames.IndexOf(name);

    /// <summary>
    /// Returns a <see cref="T:System.Data.DataTable" /> that describes the column metadata of the <see cref="T:System.Data.IDataReader" />.
    /// Returns <see langword="null" /> if the executed command returned no resultset, or after <see cref="M:System.Data.IDataReader.NextResult" /> returns <see langword="false" />.
    /// </summary>
    /// <returns>A <see cref="T:System.Data.DataTable" /> that describes the column metadata.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual DataTable GetSchemaTable() => throw new NotImplementedException();

    /// <summary>Gets the string value of the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The string value of the specified field.</returns>
    public virtual string GetString(int i) => (string)this.currentDataRow[i];

    /// <summary>Return the value of the specified field.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns>The <see cref="T:System.Object" /> which will contain the field value upon return.</returns>
    public virtual object GetValue(int i) => this.currentDataRow[i];

    /// <summary>Populates an array of objects with the column values of the current record.</summary>
    /// <param name="values">An array of <see cref="T:System.Object" /> to copy the attribute fields into.</param>
    /// <returns>The number of instances of <see cref="T:System.Object" /> in the array.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual int GetValues(object[] values) => throw new NotImplementedException();

    /// <summary>Return whether the specified field is set to null.</summary>
    /// <param name="i">The index of the field to find.</param>
    /// <returns><see langword="true" /> if the specified field is set to null; otherwise, <see langword="false" />.</returns>
    public virtual bool IsDBNull(int i) => this.currentDataRow[i] == null || this.currentDataRow[i] == DBNull.Value;

    /// <summary>Advances the data reader to the next result, when reading the results of batch SQL statements.</summary>
    /// <returns><see langword="true" /> if there are more rows; otherwise, <see langword="false" />.</returns>
    public virtual bool NextResult()
    {
        this.currentResultIndex++;

        if (this.results.Count > this.currentResultIndex)
        {
            this.currentResult = this.results[this.currentResultIndex];
            this.currentDataIndex = -1;
            this.currentDataRow = null;
            return true;
        }

        return false;
    }

    /// <summary>Advances the <see cref="T:System.Data.IDataReader" /> to the next record.</summary>
    /// <returns><see langword="true" /> if there are more rows; otherwise, <see langword="false" />.</returns>
    public virtual bool Read()
    {
        this.currentDataIndex++;

        if (this.currentResult.datarows != null && this.currentResult.datarows.Count > this.currentDataIndex)
        {
            this.currentDataRow = this.currentResult.datarows[this.currentDataIndex];
            return true;
        }

        return false;
    }

    /// <summary>Closes the <see cref="T:System.Data.IDataReader" /> Object.
    /// </summary>
    public virtual void Close() => this.IsClosed = true;

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public virtual void Dispose()
    {
        if (!this.IsClosed)
        {
            this.Close();
        }
    }
}
