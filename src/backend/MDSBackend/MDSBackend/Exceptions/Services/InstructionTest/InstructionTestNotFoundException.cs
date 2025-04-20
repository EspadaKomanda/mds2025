namespace MDSBackend.Exceptions.Services.InstructionTest;

/// <summary>
/// Represents an exception that occurs when an instruction test is not found.
/// </summary>
public class InstructionTestNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionTestNotFoundException"/> class.
    /// </summary>
    public InstructionTestNotFoundException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionTestNotFoundException"/> class with a specified error message.
    /// </summary>
   
    /// <param name="message">The message that describes the error.</param>
    public InstructionTestNotFoundException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionTestNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public InstructionTestNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

