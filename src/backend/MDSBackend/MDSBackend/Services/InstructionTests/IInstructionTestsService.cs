using MDSBackend.Models.DTO;

namespace MDSBackend.Services.InstructionTests;

public interface IInstructionTestsService
{
  public Task<InstructionTestDTO> CreateInstructionTestAsync(InstructionTestCreateDTO instructionTest);
  public InstructionTestDTO GetInstructionTestById(long id);
  public List<InstructionTestDTO> GetInstructionTestsByInstructionId(long instructionId);
  public Task<bool> UpdateInstructionTestAsync(InstructionTestCreateDTO instructionTest);
  public Task<bool> DeleteInstructionTestByIdAsync(long id);
  public Task<InstructionTestResultDTO> SubmitInstructionTestAsync(long userId, InstructionTestSubmissionDTO submission);
  public Task<List<InstructionTestResultDTO>> GetInstructionTestResultsByInstructionId(long instructionId);
  public Task<List<InstructionTestResultDTO>> GetInstructionTestResultsByUserId(long userId);
  public Task<List<InstructionTestResultDTO>> GetCompletedInstructionTestsByUserId(long userId);
}
