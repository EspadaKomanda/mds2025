namespace MDSBackend.Exceptions.UtilServices.Email;

/// <summary>
/// Represents an exception that occurs during the process of sending an email.
/// </summary>
public class SendEmailException : EmailException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SendEmailException"/> class.
    /// </summary>
    public SendEmailException() : base("An error occurred while sending the email.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SendEmailException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SendEmailException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SendEmailException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public SendEmailException(string message, Exception innerException) : base(message, innerException) { }
}