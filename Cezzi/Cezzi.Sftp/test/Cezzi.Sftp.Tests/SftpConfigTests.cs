namespace Cezzi.Sftp.Tests;

using Cezzi.Sftp;
using FluentAssertions;
using System;
using Xunit;

public class SftpConfigTests
{
    [Fact]
    public void sftpconfig___defaults_expected()
    {
        var config = new SftpConfig();

        config.ConnectRetryAttempts.Should().Be(3);
        config.ConnectTimeout.Should().Be(TimeSpan.FromMinutes(10));
        config.OperationTimeout.Should().Be(TimeSpan.FromHours(8));
        config.Port.Should().Be(22);
        config.Password.Should().Be(default);
        config.PrivateKey.Should().BeNull();
        config.Username.Should().Be(default);
        config.Hostname.Should().Be(default);
    }
}