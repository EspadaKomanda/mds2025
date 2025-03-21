namespace MDSBackend.Exceptions.UtilServices.Api;

/// <summary>
/// Represents an exception related to api operations.
/// </summary>
public class ForbiddenException : ApiException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
    /// </summary>
    public ForbiddenException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ForbiddenException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }

}
