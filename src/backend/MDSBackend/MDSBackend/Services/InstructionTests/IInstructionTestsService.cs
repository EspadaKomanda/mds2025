using MDSBackend.Models.DTO;

namespace MDSBackend.Services.InstructionTests;

public interface IInstructionTestsService
{
  public Task<InstructionTestDTO> CreateInstructionTestAsync(InstructionTestCreateDTO instructionTest);
  public InstructionTestDTO GetInstructionTestById(long id);
  public List<InstructionTestDTO> GetInstructionTestsByInstructionId(long instructionId);
  public List<InstructionTestQuestionDTO> GetInstructionTestQuestionsByInstructionTestId(long instructionTestId);
  public Task<bool> UpdateInstructionTestAsync(InstructionTestCreateDTO instructionTest);
  public Task<bool> DeleteInstructionTestByIdAsync(long id);
  public Task<InstructionTestResultDTO> SubmitInstructionTestAsync(long userId, InstructionTestSubmissionDTO submission);
  public List<InstructionTestResultDTO> GetUserInstructionTestResultsByInstructionId(long userId, long instructionId);
  public List<InstructionTestResultDTO> GetInstructionTestResultsByUserId(long userId);
  public List<InstructionTestResultDTO> GetCompletedInstructionTestsByUserId(long userId);
}
