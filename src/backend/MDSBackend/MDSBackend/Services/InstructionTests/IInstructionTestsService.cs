using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Services.InstructionTests;

// TODO: methods for test completion, viewing results, checking rights,
// updating questions, checking user results
public interface IInstructionTestsService
{
  public Task<InstructionTest> CreateInstructionTestAsync(InstructionTestCreateDTO instructionTest);
  public InstructionTest GetInstructionTestById(long id);
  public List<InstructionTest> GetInstructionTestsByInstructionId(long instructionId);
  public Task<bool> UpdateInstructionTestAsync(InstructionTestCreateDTO instructionTest);
  public Task<bool> DeleteInstructionTestByIdAsync(long id);
}
