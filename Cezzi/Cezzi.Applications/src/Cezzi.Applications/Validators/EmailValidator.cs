namespace Cezzi.Applications.Validators;

using System;
using System.Text.RegularExpressions;

/// <summary>
/// 
/// </summary>
public static class EmailValidator
{
    private const string InternetEmailRegex = @"^.+@.+\..{2,}$";

    /// <summary>Uses the Microsoft MailAddress validator.  Does not allow intranet based emails
    /// with only local@domain format.</summary>
    /// <param name="email">The email.</param>
    /// <returns></returns>
    public static bool Validate(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        if (!Regex.IsMatch(email, InternetEmailRegex))
        {
            return false;
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email.Trim();
        }
        catch (Exception)
        {
            return false;
        }
    }
}
