namespace MDSBackend.Exceptions.Services.JwtService;

/// <summary>
/// Represents an exception related to jwt token service operations.
/// </summary>
public class JwtServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JwtServiceException"/> class.
    /// </summary>
    public JwtServiceException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtServiceException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public JwtServiceException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtServiceException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public JwtServiceException(string message, Exception innerException) : base(message, innerException) { }

}