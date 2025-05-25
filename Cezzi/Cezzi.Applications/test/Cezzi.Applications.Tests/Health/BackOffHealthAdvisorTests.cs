namespace Cezzi.Applications.Tests.Health;

using Cezzi.Applications.Health;
using FluentAssertions;
using System;
using System.Threading;
using Xunit;

public class BackOffHealthAdvisorTests
{
    [Fact]
    public void backoff_healthadvisor___reports_correctly()
    {
        var enteredCount = 0;
        var exitedCount = 0;
        var reportedCount = 0;

        var advisor = new BackOffHealthAdvisor(
            maxFailureCount: 5,
            unHealthyDuration: TimeSpan.FromSeconds(120),
            extendDurationOnImmediateFailure: false,
            onEnterUnHealthyState: () => enteredCount++,
            onExitUnHealthyState: () => exitedCount++,
            onReportingUnHealthy: () => reportedCount++);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(1);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(2);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(3);

        advisor.RecordSuccess();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(3);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void backoff_healthadvisor___defaults_max_failure_count(int maxFailureCount)
    {
        var enteredCount = 0;
        var exitedCount = 0;
        var reportedCount = 0;

        var advisor = new BackOffHealthAdvisor(
            maxFailureCount: maxFailureCount,
            unHealthyDuration: TimeSpan.FromSeconds(120),
            extendDurationOnImmediateFailure: false,
            onEnterUnHealthyState: () => enteredCount++,
            onExitUnHealthyState: () => exitedCount++,
            onReportingUnHealthy: () => reportedCount++);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(1);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(2);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(3);

        advisor.RecordSuccess();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(3);
    }

    [Fact]
    public void backoff_healthadvisor___exits_unhealthystate_after_duration()
    {
        var enteredCount = 0;
        var exitedCount = 0;
        var reportedCount = 0;

        var advisor = new BackOffHealthAdvisor(
            maxFailureCount: 1,
            unHealthyDuration: TimeSpan.FromSeconds(2),
            extendDurationOnImmediateFailure: false,
            onEnterUnHealthyState: () => enteredCount++,
            onExitUnHealthyState: () => exitedCount++,
            onReportingUnHealthy: () => reportedCount++);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(1);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(2);

        Thread.Sleep(2001);

        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(2);
    }

    [Fact]
    public void backoff_healthadvisor___with_no_immediate_reenter_on_failure()
    {
        var enteredCount = 0;
        var exitedCount = 0;
        var reportedCount = 0;

        var advisor = new BackOffHealthAdvisor(
            maxFailureCount: 3,
            unHealthyDuration: TimeSpan.FromSeconds(2),
            extendDurationOnImmediateFailure: false,
            onEnterUnHealthyState: () => enteredCount++,
            onExitUnHealthyState: () => exitedCount++,
            onReportingUnHealthy: () => reportedCount++);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(1);

        Thread.Sleep(2001);

        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(1);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(1);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(1);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(2);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(2);
    }

    [Fact]
    public void backoff_healthadvisor___with_immediate_reenter_on_failure()
    {
        var enteredCount = 0;
        var exitedCount = 0;
        var reportedCount = 0;

        var advisor = new BackOffHealthAdvisor(
            maxFailureCount: 3,
            unHealthyDuration: TimeSpan.FromSeconds(2),
            extendDurationOnImmediateFailure: true,
            onEnterUnHealthyState: () => enteredCount++,
            onExitUnHealthyState: () => exitedCount++,
            onReportingUnHealthy: () => reportedCount++);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(1);

        Thread.Sleep(2001);

        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(1);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(2);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(2);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(2);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(3);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(2);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(4);
    }

    [Fact]
    public void backoff_healthadvisor___only_reports_health_reenter_once()
    {
        var enteredCount = 0;
        var exitedCount = 0;
        var reportedCount = 0;

        var advisor = new BackOffHealthAdvisor(
            maxFailureCount: 3,
            unHealthyDuration: TimeSpan.FromSeconds(2),
            extendDurationOnImmediateFailure: true,
            onEnterUnHealthyState: () => enteredCount++,
            onExitUnHealthyState: () => exitedCount++,
            onReportingUnHealthy: () => reportedCount++);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.RecordFailure();
        advisor.IsHealthy().Should().BeFalse();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(1);

        Thread.Sleep(2001);

        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(1);

        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(1);

        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(1);

        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(1);

        advisor.RecordSuccess();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(1);

        advisor.RecordSuccess();
        enteredCount.Should().Be(1);
        exitedCount.Should().Be(1);
        reportedCount.Should().Be(1);
    }

    [Fact]
    public void backoff_healthadvisor___doesnt_eport_healthy_if_never_unhealthy()
    {
        var enteredCount = 0;
        var exitedCount = 0;
        var reportedCount = 0;

        var advisor = new BackOffHealthAdvisor(
            maxFailureCount: 3,
            unHealthyDuration: TimeSpan.FromSeconds(2),
            extendDurationOnImmediateFailure: true,
            onEnterUnHealthyState: () => enteredCount++,
            onExitUnHealthyState: () => exitedCount++,
            onReportingUnHealthy: () => reportedCount++);

        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);

        advisor.IsHealthy().Should().BeTrue();
        enteredCount.Should().Be(0);
        exitedCount.Should().Be(0);
        reportedCount.Should().Be(0);
    }
}
