namespace MDSBackend.Exceptions.UtilServices.Email;

/// <summary>
/// Represents an exception related to email operations.
/// </summary>
public class EmailException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailException"/> class.
    /// </summary>
    public EmailException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EmailException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public EmailException(string message, Exception innerException) : base(message, innerException) { }
}