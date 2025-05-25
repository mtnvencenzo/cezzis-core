namespace Cezzi.SendGrid;

/// <summary>
/// 
/// </summary>
public class EmailValue
{
    /// <summary>Initializes a new instance of the <see cref="EmailValue"/> class.</summary>
    /// <param name="email">The email.</param>
    public EmailValue(string email)
    {
        this.Email = email;
    }

    /// <summary>Emails the value.</summary>
    /// <param name="email">The email.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public EmailValue(string email, string name)
    {
        this.Email = email;
        this.Name = name;
    }

    /// <summary>Gets or sets the email.</summary>
    /// <value>The email.</value>
    public string Email { get; }

    /// <summary>Gets or sets the name.</summary>
    /// <value>The name.</value>
    public string Name { get; }
}
