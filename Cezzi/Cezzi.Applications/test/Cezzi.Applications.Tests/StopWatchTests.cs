namespace Cezzi.Applications.Tests;

using Cezzi.Applications;
using FluentAssertions;
using Xunit;

public class StopWatchTests
{
    [Fact]
    public void stopwatch___elapses_correctly()
    {
        var sw = new StopWatch();

        System.Threading.Thread.Sleep(100);

        var elapsed = sw.Elapsed();
        elapsed.Should().BeGreaterThanOrEqualTo(100);
        elapsed.Should().BeLessThan(1000);
    }

    [Fact]
    public void stopwatch___elapses_correctly_with_reset()
    {
        var sw = new StopWatch();

        System.Threading.Thread.Sleep(1000);

        var elapsed = sw.Elapsed(true);
        var elapsed2 = sw.Elapsed();
        elapsed.Should().BeGreaterThanOrEqualTo(100);
        elapsed.Should().BeLessThan(2000);

        elapsed2.Should().BeLessThan(elapsed);
    }

    [Fact]
    public void stopwatch___elapses_correctly_after_reset()
    {
        var sw = new StopWatch();
        System.Threading.Thread.Sleep(100);

        var elapsed = sw.Elapsed();
        elapsed.Should().BeGreaterThanOrEqualTo(100);
        elapsed.Should().BeLessThan(1000);

        sw.Reset();
        System.Threading.Thread.Sleep(100);

        elapsed = sw.Elapsed();
        elapsed.Should().BeGreaterThanOrEqualTo(100);
        elapsed.Should().BeLessThan(1000);
    }
}
