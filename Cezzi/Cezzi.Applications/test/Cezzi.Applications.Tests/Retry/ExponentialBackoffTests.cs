namespace Cezzi.Applications.Tests.Retry;

using Cezzi.Applications;
using Cezzi.Applications.Retry;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

public class ExponentialBackoffTests
{
    [Fact]
    public async Task expbackoff___executeasync_with_return_value_has_correct_backoffs()
    {
        var backoff = new ExponentialBackoff();

        var attempts = 0;
        var firstElapsed = 0L;
        var secondElapsed = 0L;
        var thridElapsed = 0L;
        var sw = new StopWatch();

        var result = await backoff.ExecuteAsync(
            maxAttempts: 3,
            backOffMilliseconds: 300,
            maxBackOffMilliseconds: 1200,
            func: async () =>
            {
                attempts++;

                if (attempts == 1)
                {
                    firstElapsed = sw.Elapsed();
                }
                else if (attempts == 2)
                {
                    secondElapsed = sw.Elapsed();
                }
                else if (attempts == 3)
                {
                    thridElapsed = sw.Elapsed();
                    return await Task.FromResult(0).ConfigureAwait(false);
                }

                throw new Exception("Force Fail");
            }).ConfigureAwait(false);

        result.Should().Be(0);

        firstElapsed.Should().BeLessThan(300);
        secondElapsed.Should().BeLessThanOrEqualTo(600);
        secondElapsed.Should().BeGreaterThanOrEqualTo(300);
        thridElapsed.Should().BeLessThanOrEqualTo(1200);
        thridElapsed.Should().BeGreaterThanOrEqualTo(600);
    }

    [Fact]
    public async Task expbackoff___executeasync_with_return_throws_when_max_attemps_reached()
    {
        var backoff = new ExponentialBackoff();

        var attempts = 0;
        var firstElapsed = 0L;
        var secondElapsed = 0L;
        var thridElapsed = 0L;
        var sw = new StopWatch();

        var ex = await Assert.ThrowsAsync<Exception>(async () => _ = await backoff.ExecuteAsync(
            maxAttempts: 3,
            backOffMilliseconds: 300,
            maxBackOffMilliseconds: 1200,
            func: async () =>
            {
                attempts++;

                if (attempts == 1)
                {
                    firstElapsed = sw.Elapsed();
                    throw new Exception("Force Fail 1");
                }
                else if (attempts == 2)
                {
                    secondElapsed = sw.Elapsed();
                    throw new Exception("Force Fail 2");
                }
                else if (attempts == 3)
                {
                    thridElapsed = sw.Elapsed();
                    throw new Exception("Force Fail 3");
                }

                return await Task.FromResult(0).ConfigureAwait(false);

            }).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Force Fail 3");

        firstElapsed.Should().BeLessThan(300);
        secondElapsed.Should().BeLessThanOrEqualTo(600);
        secondElapsed.Should().BeGreaterThanOrEqualTo(300);
        thridElapsed.Should().BeLessThanOrEqualTo(1200);
        thridElapsed.Should().BeGreaterThanOrEqualTo(600);
    }

    [Fact]
    public async Task expbackoff___executeasync_with_void_value_has_correct_backoffs()
    {
        var backoff = new ExponentialBackoff();

        var attempts = 0;
        var firstElapsed = 0L;
        var secondElapsed = 0L;
        var thridElapsed = 0L;
        var sw = new StopWatch();

        await backoff.ExecuteAsync(
            maxAttempts: 3,
            backOffMilliseconds: 300,
            maxBackOffMilliseconds: 1200,
            func: async () =>
            {
                attempts++;

                if (attempts == 1)
                {
                    firstElapsed = sw.Elapsed();
                }
                else if (attempts == 2)
                {
                    secondElapsed = sw.Elapsed();
                }
                else if (attempts == 3)
                {
                    thridElapsed = sw.Elapsed();

                    _ = await Task.FromResult(0).ConfigureAwait(false);
                    return;
                }

                throw new Exception("Force Fail");
            }).ConfigureAwait(false);

        firstElapsed.Should().BeLessThan(300);
        secondElapsed.Should().BeLessThanOrEqualTo(600);
        secondElapsed.Should().BeGreaterThanOrEqualTo(300);
        thridElapsed.Should().BeLessThanOrEqualTo(1200);
        thridElapsed.Should().BeGreaterThanOrEqualTo(600);
    }

    [Fact]
    public async Task expbackoff___executeasync_with_void_throws_when_max_attemps_reached()
    {
        var backoff = new ExponentialBackoff();

        var attempts = 0;
        var firstElapsed = 0L;
        var secondElapsed = 0L;
        var thridElapsed = 0L;
        var sw = new StopWatch();

        var ex = await Assert.ThrowsAsync<Exception>(async () => await backoff.ExecuteAsync(
            maxAttempts: 3,
            backOffMilliseconds: 300,
            maxBackOffMilliseconds: 1200,
            func: async () =>
            {
                attempts++;

                if (attempts == 1)
                {
                    firstElapsed = sw.Elapsed();
                    throw new Exception("Force Fail 1");
                }
                else if (attempts == 2)
                {
                    secondElapsed = sw.Elapsed();
                    throw new Exception("Force Fail 2");
                }
                else if (attempts == 3)
                {
                    thridElapsed = sw.Elapsed();
                    throw new Exception("Force Fail 3");
                }

                _ = await Task.FromResult(0).ConfigureAwait(false);
                return;

            }).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Force Fail 3");

        firstElapsed.Should().BeLessThan(300);
        secondElapsed.Should().BeLessThanOrEqualTo(600);
        secondElapsed.Should().BeGreaterThanOrEqualTo(300);
        thridElapsed.Should().BeLessThanOrEqualTo(1200);
        thridElapsed.Should().BeGreaterThanOrEqualTo(600);
    }
}
