namespace PurpleHackBackend.Exceptions.UtilServices.JWT;

/// <summary>
/// Represents an exception that occurs while generating a JWT token.
/// </summary>
public class GenerateJWTTokenException : JWTException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateJWTTokenException"/> class.
    /// </summary>
    public GenerateJWTTokenException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateJWTTokenException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public GenerateJWTTokenException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateJWTTokenException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public GenerateJWTTokenException(string message, Exception innerException) : base(message, innerException) { }
}