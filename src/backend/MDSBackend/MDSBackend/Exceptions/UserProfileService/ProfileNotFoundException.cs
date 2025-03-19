namespace MDSBackend.Exceptions.Services.ProfileService;

/// <summary>
/// Represents an exception that occurs when a profile is not found.
/// </summary>
public class ProfileNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileNotFoundException"/> class.
    /// </summary>
    public ProfileNotFoundException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ProfileNotFoundException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ProfileNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

