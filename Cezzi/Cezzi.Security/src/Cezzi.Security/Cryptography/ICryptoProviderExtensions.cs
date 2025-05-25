namespace Cezzi.Security.Cryptography;

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public static class AesCryptoProviderExtensions
{
    /// <summary>Encrypts the property.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item">The item.</param>
    /// <param name="expression">The expression.</param>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">cryptoProvider</exception>
    /// <exception cref="ArgumentException">
    /// </exception>
    public static T EncryptProperty<T>(this T item,
        Expression<Func<T, string>> expression,
        ICryptoProvider cryptoProvider)
    {
        ArgumentNullException.ThrowIfNull(cryptoProvider);

        if (item == null)
        {
            return item;
        }

        var member = expression.Body as MemberExpression ?? throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", expression));
        var propInfo = member.Member as PropertyInfo ?? throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", expression));

        if (!propInfo.CanRead)
        {
            throw new ArgumentException(string.Format("Expression '{0}' refers to a non-readable property.", expression));
        }

        if (!propInfo.CanWrite)
        {
            throw new ArgumentException(string.Format("Expression '{0}' refers to a non-writeable property.", expression));
        }

        // Need to get the parent object so we can get/set the intance value
        var parentEx = ((MemberExpression)expression.Body).Expression;
        var parentLambda = Expression.Lambda<Func<T, object>>(parentEx, expression.Parameters[0]);
        var parent = parentLambda.Compile().Invoke(item);

        if (parent != null)
        {
            // Now get the value and encrypt it and set it back on the property

            if (propInfo.GetGetMethod().Invoke(parent, []) is string propValue)
            {
                var encrypted = cryptoProvider.Encrypt(propValue);
                propInfo.SetValue(parent, encrypted);
            }
        }

        return item;
    }

    /// <summary>Encrypts the property.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item">The item.</param>
    /// <param name="expression">The expression.</param>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">cryptoProvider</exception>
    /// <exception cref="ArgumentException">
    /// </exception>
    public static Task<T> EncryptPropertyAsync<T>(this T item,
        Expression<Func<T, string>> expression,
        ICryptoProvider cryptoProvider) => Task.FromResult(item.EncryptProperty(expression, cryptoProvider));

    /// <summary>Decrypts the property.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item">The item.</param>
    /// <param name="expression">The expression.</param>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">cryptoProvider</exception>
    /// <exception cref="ArgumentException">
    /// </exception>
    public static T DecryptProperty<T>(this T item,
        Expression<Func<T, string>> expression,
        ICryptoProvider cryptoProvider)
    {
        ArgumentNullException.ThrowIfNull(cryptoProvider);

        if (item == null)
        {
            return item;
        }

        var member = expression.Body as MemberExpression ?? throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", expression));
        var propInfo = member.Member as PropertyInfo ?? throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", expression));

        if (!propInfo.CanRead)
        {
            throw new ArgumentException(string.Format("Expression '{0}' refers to a non-readable property.", expression));
        }

        if (!propInfo.CanWrite)
        {
            throw new ArgumentException(string.Format("Expression '{0}' refers to a non-writeable property.", expression));
        }

        // Need to get the parent object so we can get/set the intance value
        var parentEx = ((MemberExpression)expression.Body).Expression;
        var parentLambda = Expression.Lambda<Func<T, object>>(parentEx, expression.Parameters[0]);
        var parent = parentLambda.Compile().Invoke(item);

        if (parent != null)
        {
            // Now get the value and decrypt it and set it back on the property

            if (propInfo.GetGetMethod().Invoke(parent, []) is string propValue)
            {
                var decrypted = cryptoProvider.Decrypt(propValue);
                propInfo.SetValue(parent, decrypted);
            }
        }

        return item;
    }

    /// <summary>Decrypts the property.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item">The item.</param>
    /// <param name="expression">The expression.</param>
    /// <param name="cryptoProvider">The crypto provider.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">cryptoProvider</exception>
    /// <exception cref="ArgumentException">
    /// </exception>
    public static Task<T> DecryptPropertyAsync<T>(this T item,
        Expression<Func<T, string>> expression,
        ICryptoProvider cryptoProvider) => Task.FromResult(item.DecryptProperty(expression, cryptoProvider));
}
