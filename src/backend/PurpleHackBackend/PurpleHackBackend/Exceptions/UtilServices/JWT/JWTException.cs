namespace PurpleHackBackend.Exceptions.UtilServices.JWT;

/// <summary>
/// Represents an exception related to JWT (JSON Web Token) operations.
/// </summary>
public class JWTException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JWTException"/> class.
    /// </summary>
    public JWTException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="JWTException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public JWTException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="JWTException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public JWTException(string message, Exception innerException) : base(message, innerException) { }
}