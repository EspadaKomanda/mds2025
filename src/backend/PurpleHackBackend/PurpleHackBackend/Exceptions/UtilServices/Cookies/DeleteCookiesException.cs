namespace PurpleHackBackend.Exceptions.UtilServices.Cookies;

/// <summary>
/// Represents an exception related to deleting cookies.
/// </summary>
public class DeleteCookiesException : CookiesException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCookiesException"/> class.
    /// </summary>
    public DeleteCookiesException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCookiesException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DeleteCookiesException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCookiesException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public DeleteCookiesException(string message, Exception innerException) : base(message, innerException) { }

}