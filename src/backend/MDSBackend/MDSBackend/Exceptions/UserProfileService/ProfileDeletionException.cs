namespace MDSBackend.Exceptions.Services.ProfileService;

/// <summary>
/// Represents an exception that occurs during profile deletion operations.
/// </summary>
public class ProfileDeletionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileDeletionException"/> class.
    /// </summary>
    public ProfileDeletionException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileDeletionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ProfileDeletionException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileDeletionException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ProfileDeletionException(string message, Exception innerException) : base(message, innerException) { }
}

