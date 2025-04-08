using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;
using MDSBackend.Models.Messages.Instructions;

namespace MDSBackend.Services.Instructions;

public interface IInstructionService
{
    public Task<Instruction> CreateInstruction(Instruction model);
    public Task<bool> UpdateInstructionById(Instruction model);
    public Task<bool> DeleteInstructionById(long instructionId);

    public Task<InstructionDTO> CreateInstruction(CreateInstructionRequest model);
    public Task<bool> UpdateInstructionById(UpdateInstructionRequest model);

    public List<InstructionDTO> GetAllInstructions(long userId);
    public List<InstructionDTO> GetInstructionsByCategoryId(long userId, long categoryId);
    public List<InstructionDTO> GetCompletedInstructions(long userId);
    public List<InstructionDTO> GetUnfinishedInstructions(long userId);
    public List<InstructionDTO> GetInstructionById(long userId, long instructionId);
}
