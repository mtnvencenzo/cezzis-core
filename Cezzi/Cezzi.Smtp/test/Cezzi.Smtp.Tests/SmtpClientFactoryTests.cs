namespace Cezzi.Smtp.Tests;

using FluentAssertions;

public class SmtpClientFactoryTests
{
    [Fact]
    public void smtpclientfactory__returns_standard_smtp_client_no_host_or_port()
    {
        var client = new SmtpClientFactory().CreateClient();

        client.Host.Should().BeNull();
        client.Port.Should().Be(25);
    }

    [Fact]
    public void smtpclientfactory__returns_standard_smtp_client_with_host_no_port()
    {
        var client = new SmtpClientFactory().CreateClient("smtp.cezzi.com");

        client.Host.Should().Be("smtp.cezzi.com");
        client.Port.Should().Be(25);
    }

    [Fact]
    public void smtpclientfactory__returns_standard_smtp_client_with_host_and_port()
    {
        var client = new SmtpClientFactory().CreateClient("smtp.cezzi.com", 99809);

        client.Host.Should().Be("smtp.cezzi.com");
        client.Port.Should().Be(99809);
    }
}