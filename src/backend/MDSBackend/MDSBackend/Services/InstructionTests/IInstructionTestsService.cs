using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Services.InstructionTests;

public interface IInstructionTestsService
{
  public Task<InstructionTest> CreateInstructionTest(InstructionTest instructionTest);
  public Task<InstructionTestDTO> CreateInstructionTest(InstructionTestCreateDTO instructionTest);
  public Task<bool> UpdateInstructionTest(InstructionTest instructionTest);
  public Task<bool> UpdateInstructionTest(InstructionTestCreateDTO instructionTest);
  public Task<bool> DeleteInstructionTestByIdAsync(long id);

  public Task<InstructionTestResultDTO> SubmitInstructionTestAsync(long userId, InstructionTestSubmissionDTO submission);

  public InstructionTestDTO GetInstructionTestById(long id);
  public List<InstructionTestDTO> GetInstructionTestsByInstructionId(long instructionId);
  public List<InstructionTestQuestionDTO> GetInstructionTestQuestionsByInstructionTestId(long instructionTestId);
  public List<InstructionTestResultDTO> GetUserInstructionTestResultsByInstructionTestId(long userId, long instructionId);
  public List<InstructionTestResultDTO> GetInstructionTestResultsByUserId(long userId);
  public List<InstructionTestResultDTO> GetCompletedInstructionTestsByUserId(long userId);
}
