namespace Cezzi.Sftp.Tests;

using Cezzi.Sftp;
using FluentAssertions;
using System;
using Xunit;

public class SftpExceptionTests
{
    [Fact]
    public void sftpexception___tostring()
    {
        var exception = new SftpException();
        exception.Data.Add("test1", "test-vaule1");
        exception.Data.Add("test2", "test-vaule2");

        var result = exception.ToString();
        result.Should().Contain("test1 at test-vaule1");
        result.Should().Contain("test2 at test-vaule2");
    }

    [Fact]
    public void sftpexception___tostring_with_message()
    {
        var exception = new SftpException("test-message");
        exception.Data.Add("test1", "test-vaule1");
        exception.Data.Add("test2", "test-vaule2");

        var result = exception.ToString();
        result.Should().Contain("test-message");
        result.Should().Contain("test1 at test-vaule1");
        result.Should().Contain("test2 at test-vaule2");
    }

    [Fact]
    public void sftpexception___tostring_with_message_and_inner()
    {
        var exception = new SftpException("test-message", new ArgumentException("arg1-exception"));
        exception.Data.Add("test1", "test-vaule1");
        exception.Data.Add("test2", "test-vaule2");

        var result = exception.ToString();
        result.Should().Contain("test-message");
        result.Should().Contain("arg1-exception");
        result.Should().Contain("test1 at test-vaule1");
        result.Should().Contain("test2 at test-vaule2");
    }
}