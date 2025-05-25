namespace Cezzi.Applications.Retry;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public class ExponentialBackoff
{
    /// <summary>Executes the asynchronous.</summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="func">The function.</param>
    /// <param name="maxAttempts">The maximum attempts.</param>
    /// <param name="backOffMilliseconds">The back off milliseconds.</param>
    /// <param name="maxBackOffMilliseconds">The maximum back off milliseconds.</param>
    /// <returns></returns>
    public async Task<TValue> ExecuteAsync<TValue>(
        Func<Task<TValue>> func,
        int maxAttempts = 3,
        int backOffMilliseconds = 0,
        int maxBackOffMilliseconds = 0)
    {
        var attempts = 0;
        var currentBackOff = backOffMilliseconds > 0
            ? backOffMilliseconds
            : 0;

        while (true)
        {
            try
            {
                attempts++;
                return await func().ConfigureAwait(false);
            }
            catch
            {
                if (attempts >= maxAttempts)
                {
                    throw;
                }
                else
                {
                    if (currentBackOff > 0)
                    {
                        Thread.Sleep(currentBackOff);

                        currentBackOff = (maxBackOffMilliseconds > 0 && (currentBackOff * 2) >= maxBackOffMilliseconds)
                            ? maxBackOffMilliseconds
                            : currentBackOff * 2;
                    }
                }
            }
        }
    }

    /// <summary>Executes the asynchronous.</summary>
    /// <param name="func">The function.</param>
    /// <param name="maxAttempts">The maximum attempts.</param>
    /// <param name="backOffMilliseconds">The back off milliseconds.</param>
    /// <param name="maxBackOffMilliseconds">The maximum back off milliseconds.</param>
    public async Task ExecuteAsync(
        Func<Task> func,
        int maxAttempts = 3,
        int backOffMilliseconds = 0,
        int maxBackOffMilliseconds = 0)
    {
        var attempts = 0;
        var currentBackOff = backOffMilliseconds > 0
            ? backOffMilliseconds
            : 0;

        while (true)
        {
            try
            {
                attempts++;
                await func().ConfigureAwait(false);
                return;
            }
            catch
            {
                if (attempts >= maxAttempts)
                {
                    throw;
                }
                else
                {
                    if (currentBackOff > 0)
                    {
                        Thread.Sleep(currentBackOff);

                        currentBackOff = (maxBackOffMilliseconds > 0 && (currentBackOff * 2) >= maxBackOffMilliseconds)
                            ? maxBackOffMilliseconds
                            : currentBackOff * 2;
                    }
                }
            }
        }
    }
}
