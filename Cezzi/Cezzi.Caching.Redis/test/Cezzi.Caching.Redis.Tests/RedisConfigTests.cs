namespace Cezzi.Caching.Redis.Tests;

using FluentAssertions;
using Xunit;

public class RedisConfigTests
{
    [Fact]
    public void redisconfig___getconnectionstring_with_password()
    {
        var config = new RedisConfig
        {
            RetryMaxAttempts = 2,
            ReconnectErrorThreshold = 2,
            ReconnectMinFrequency = 3,
            ConnectionString = "test://redis-connection.io",
            Password = "super"
        };

        config.GetConnectionString().Should().Be("test://redis-connection.io,password=super");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void redisconfig___getconnectionstring_no_password(string password)
    {
        var config = new RedisConfig
        {
            RetryMaxAttempts = 2,
            ReconnectErrorThreshold = 2,
            ReconnectMinFrequency = 3,
            ConnectionString = "test://redis-connection.io",
            Password = password
        };

        config.GetConnectionString().Should().Be("test://redis-connection.io");
    }
}