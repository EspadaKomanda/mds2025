using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Services.InstructionTests;

public class InstructionTestsService : IInstructionTestsService
{
    public Task<InstructionTest> CreateInstructionTestAsync(InstructionTestCreateDTO instructionTest)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteInstructionTestByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public InstructionTest GetInstructionTestById(long id)
    {
        throw new NotImplementedException();
    }

    public List<InstructionTest> GetInstructionTestsByInstructionId(long instructionId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateInstructionTestAsync(InstructionTestCreateDTO instructionTest)
    {
        throw new NotImplementedException();
    }
}

