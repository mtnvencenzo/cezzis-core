namespace Cezzi.Sftp.Renci.Tests;

using FluentAssertions;
using System.IO;
using Xunit;

public class MemoryStreamTests
{
    [Fact]
    public void memory_stream__test_what_happens_when_seeking_an_empty_stream()
    {
        using var ms = new MemoryStream();

        ms.Seek(0, SeekOrigin.Begin);
        ms.Position = 0;

        var bytes = ms.ToArray();

        bytes.Should().BeEmpty();
    }
}