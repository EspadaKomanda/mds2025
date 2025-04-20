namespace MDSBackend.Exceptions.Services.InstructionTest;

/// <summary>
/// Represents an exception that occurs when there is a conflict in instruction test operations.
/// </summary>
public class InstructionTestConflictException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionTestConflictException"/> class.
    /// </summary>
    public InstructionTestConflictException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionTestConflictException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InstructionTestConflictException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionTestConflictException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public InstructionTestConflictException(string message, Exception innerException) : base(message, innerException) { }
}

