namespace Cezzi.Applications;

using System;

/// <summary>
/// 
/// </summary>
public class StopWatch
{
    private long milliseconds;

    /// <summary>Initializes a new instance of the <see cref="StopWatch"/> class.</summary>
    public StopWatch()
    {
        this.Reset();
    }

    /// <summary>Gets the elapsed milliseconds since the stop watch was created.</summary>
    /// <param name="reset">if set to <c>true</c> [reset].</param>
    /// <returns></returns>
    public long Elapsed(bool reset = false)
    {
        var elapsed = DateTimeOffset.Now.ToUnixTimeMilliseconds() - this.milliseconds;

        if (reset)
        {
            this.Reset();
        }

        return elapsed;
    }

    /// <summary>Resets this instance.</summary>
    public void Reset() => this.milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
}
