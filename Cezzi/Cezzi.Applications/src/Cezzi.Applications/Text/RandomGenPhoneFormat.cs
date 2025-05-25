namespace Cezzi.Applications.Text;

/// <summary>
/// 
/// </summary>
public enum RandomGenPhoneFormat
{
    /// <summary>No characters just numbers</summary>
    /// <example>3334445555</example>
    None = 0,

    /// <summary>Area code surrounded with parenthesis and dahses between first 3 and last 4</summary>
    /// <example>(333) 444-5555</example>
    Full = 1,

    /// <summary>No parenthesis around area code but dashes between first 3 and second 3 and last 4</summary>
    /// <example>333-444-5555</example>
    Dashes = 2,

    /// <summary>No parenthesis around area code but dots between first 3 and second 3 and last 4</summary>
    /// <example>333.444.5555</example>
    Dots = 4
}
