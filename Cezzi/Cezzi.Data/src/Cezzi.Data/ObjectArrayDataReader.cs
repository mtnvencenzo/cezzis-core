namespace Cezzi.Data;

using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Data.ObjectArrayDataReaderBase" />
public class ObjectArrayDataReader : ObjectArrayDataReaderBase
{
    /// <summary>Initializes a new instance of the <see cref="ObjectArrayDataReader"/> class.</summary>
    /// <param name="columnNames">The column names.</param>
    /// <param name="datarows">The datarows.</param>
    /// <exception cref="System.ArgumentNullException">
    /// columnNames
    /// or
    /// datarows
    /// </exception>
    public ObjectArrayDataReader(IList<string> columnNames, IList<object[]> datarows)
    {
        _ = this.AddResultInternal(
            columnNames: columnNames ?? throw new ArgumentNullException(nameof(columnNames)),
            datarows: datarows ?? throw new ArgumentNullException(nameof(datarows)));
    }

    /// <summary>Adds the result.</summary>
    /// <param name="columnNames">The column names.</param>
    /// <param name="datarows">The datarows.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">
    /// columnNames
    /// or
    /// datarows
    /// </exception>
    public ObjectArrayDataReader AddResult(IList<string> columnNames, IList<object[]> datarows)
    {
        _ = this.AddResultInternal(
            columnNames: columnNames ?? throw new ArgumentNullException(nameof(columnNames)),
            datarows: datarows ?? throw new ArgumentNullException(nameof(datarows)));

        return this;
    }
}
