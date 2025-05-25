namespace Cezzi.Applications.IO;

using System.IO;
using System.IO.Compression;

/// <summary>
/// 
/// </summary>
public static class GZip
{
    /// <summary>
    /// Compresses the specified uncompressed bytes.
    /// </summary>
    /// <param name="uncompressedBytes">The uncompressed bytes.</param>
    /// <returns></returns>
    public static byte[] Compress(byte[] uncompressedBytes)
    {
        var compressedBytes = new byte[] { };

        using (var uncompressed = new MemoryStream(uncompressedBytes))
        {
            using var compressed = new MemoryStream();
            using (var gz = new GZipStream(compressed, CompressionMode.Compress, true))
            {
                uncompressed.CopyTo(gz);
            }

            compressed.Seek(0, SeekOrigin.Begin);
            compressedBytes = compressed.ToArray();
        }

        return compressedBytes;
    }

    /// <summary>
    /// Des the compress.
    /// </summary>
    /// <param name="compressedBytes">The compressed bytes.</param>
    /// <returns></returns>
    public static byte[] DeCompress(byte[] compressedBytes)
    {
        var unCompressedBytes = new byte[] { };

        using var unCompressed = new MemoryStream();
        using var compressed = new MemoryStream(compressedBytes);
        using var gz = new GZipStream(compressed, CompressionMode.Decompress, true);

        gz.CopyTo(unCompressed);

        unCompressed.Seek(0, SeekOrigin.Begin);
        unCompressedBytes = unCompressed.ToArray();

        return unCompressedBytes;
    }
}
