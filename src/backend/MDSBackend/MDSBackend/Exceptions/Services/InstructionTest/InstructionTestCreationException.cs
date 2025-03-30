namespace MDSBackend.Exceptions.Services.InstructionTest;

/// <summary>
/// Represents an exception that occurs during the creation of an instruction test.
/// </summary>
public class InstructionTestCreationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionTestCreationException"/> class.
    /// </summary>
    public InstructionTestCreationException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionTestCreationException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InstructionTestCreationException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionTestCreationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public InstructionTestCreationException(string message, Exception innerException) : base(message, innerException) { }
}

