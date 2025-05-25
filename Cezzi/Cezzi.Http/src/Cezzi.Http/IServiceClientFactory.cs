namespace Cezzi.Http;

/// <summary>
/// 
/// </summary>
/// <typeparam name="I"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IServiceClientFactory<I, T>
{
    /// <summary>Gets the instance.</summary>
    /// <returns></returns>
    I GetInstance();
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IServiceClientFactory<T>
{
    /// <summary>Gets the instance.</summary>
    /// <returns></returns>
    T GetInstance();
}
