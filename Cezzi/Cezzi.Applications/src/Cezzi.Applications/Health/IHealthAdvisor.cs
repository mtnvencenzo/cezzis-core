namespace Cezzi.Applications.Health;

/// <summary>
/// 
/// </summary>
public interface IHealthAdvisor
{
    /// <summary>Checks the health.</summary>
    /// <returns></returns>
    bool IsHealthy();

    /// <summary>Records the failure.</summary>
    /// <returns></returns>
    void RecordFailure();

    /// <summary>Records the success.</summary>
    /// <returns></returns>
    void RecordSuccess();
}
