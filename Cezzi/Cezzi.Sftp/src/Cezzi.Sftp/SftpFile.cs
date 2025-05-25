namespace Cezzi.Sftp;

using System;

/// <summary> Sftp file details </summary>
public class SftpFile
{
    /// <summary>Gets or sets the file name.</summary>
    /// <value>The file name.</value>
    public string Name { get; set; }

    /// <summary>Gets or sets the file full name.</summary>
    /// <value>The file full name.</value>
    public string FullName { get; set; }

    /// <summary>Gets or sets the file contents length.</summary>
    /// <value>The file contents length.</value>
    public long Length { get; set; }

    /// <summary>Gets or sets the last write date/time the file was created/updated on.</summary>
    /// <value>The last write time.</value>
    public DateTime LastWriteTime { get; set; }

    /// <summary>Gets or sets the last access time.</summary>
    /// <value>The last access time.</value>
    public DateTime LastAccessTime { get; set; }

    /// <summary>Gets or sets a value indicating whether this instance is directory.</summary>
    /// <value><c>true</c> if this instance is directory; otherwise, <c>false</c>.</value>
    public bool IsDirectory { get; set; }
}
