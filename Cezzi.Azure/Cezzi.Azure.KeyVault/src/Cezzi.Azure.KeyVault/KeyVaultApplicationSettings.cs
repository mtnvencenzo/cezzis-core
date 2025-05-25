namespace Cezzi.Azure.KeyVault;

/// <summary>
/// 
/// </summary>
public class KeyVaultApplicationSettings
{
    /// <summary>The section name</summary>
    public const string SectionName = "KeyVaultSettings";

    /// <summary>Gets or sets a value indicating whether [use key vault].</summary>
    /// <value><c>true</c> if [use key vault]; otherwise, <c>false</c>.</value>
    public virtual bool UseKeyVaultSecrets { get; set; }

    /// <summary>Gets or sets the key vault key prefix.</summary>
    /// <value>The key vault key prefix.</value>
    public virtual string KeyVaultKeyPrefix { get; set; }

    /// <summary>Gets or sets the key vault base URI.</summary>
    /// <value>The key vault base URI.</value>
    public virtual string KeyVaultBaseUri { get; set; }
}