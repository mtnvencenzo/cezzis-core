namespace Cezzi.Applications.Health;

using System;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Applications.Health.IHealthAdvisor" />
/// <remarks>Initializes a new instance of the <see cref="BackOffHealthAdvisor" /> class.</remarks>
/// <param name="maxFailureCount">The maximum failure count.</param>
/// <param name="unHealthyDuration">Duration of the un healthy.</param>
/// <param name="extendDurationOnImmediateFailure">if set to <c>true</c> [extend duration on immediate failure].</param>
/// <param name="onEnterUnHealthyState">State of the on enter un healthy.</param>
/// <param name="onExitUnHealthyState">State of the on exit un healthy.</param>
/// <param name="onReportingUnHealthy">The on reporting un healthy.</param>
public class BackOffHealthAdvisor(
    int maxFailureCount,
    TimeSpan unHealthyDuration,
    bool extendDurationOnImmediateFailure = false,
    Action onEnterUnHealthyState = null,
    Action onExitUnHealthyState = null,
    Action onReportingUnHealthy = null) : IHealthAdvisor
{
    private readonly int maxFailureCount = maxFailureCount > 0
            ? maxFailureCount
            : 3;
    private readonly TimeSpan unHealthyDuration = unHealthyDuration.TotalMilliseconds > 0
            ? unHealthyDuration
            : TimeSpan.FromSeconds(60);
    private readonly bool extendDurationOnImmediateFailure = extendDurationOnImmediateFailure;
    private readonly Action onEnterUnHealthyState = onEnterUnHealthyState;
    private readonly Action onExitUnHealthyState = onExitUnHealthyState;
    private readonly Action onReportingUnHealthy = onReportingUnHealthy;
    private DateTime? unHeathyEnd = null;
    private int failureCount = 0;

    /// <summary>Checks the health.</summary>
    /// <returns></returns>
    public virtual bool IsHealthy()
    {
        if (this.unHeathyEnd.HasValue)
        {
            if (DateTime.Now <= this.unHeathyEnd.Value)
            {
                this.onReportingUnHealthy?.Invoke();
                return false;
            }

            this.Clear(clearFailureCount: !this.extendDurationOnImmediateFailure);
        }

        return true;
    }

    /// <summary>Records the failure.</summary>
    public void RecordFailure()
    {
        this.failureCount++;

        if (this.failureCount >= this.maxFailureCount)
        {
            var isEntering = this.unHeathyEnd.HasValue == false;

            this.unHeathyEnd = DateTime.Now + this.unHealthyDuration;

            if (isEntering)
            {
                this.onEnterUnHealthyState?.Invoke();
            }
        }
    }

    /// <summary>Records the success.</summary>
    public void RecordSuccess() => this.Clear(clearFailureCount: true);

    private void Clear(bool clearFailureCount = true)
    {
        if (clearFailureCount)
        {
            this.failureCount = 0;
        }

        if (this.unHeathyEnd.HasValue)
        {
            this.unHeathyEnd = null;

            this.onExitUnHealthyState?.Invoke();
        }
    }
}
