using AutoMapper;
using MDSBackend.Database.Repositories;
using MDSBackend.Exceptions.Services.Instruction;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;
using MDSBackend.Services.InstructionTests;

namespace MDSBackend.Services.Instructions;

public class InstructionService : IInstructionService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<InstructionService> _logger;
    private readonly IInstructionTestsService _instructionTestService;

    public InstructionService(UnitOfWork unitOfWork, IMapper mapper, ILogger<InstructionService> logger, IInstructionTestsService instructionTestService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _instructionTestService = instructionTestService;
    }

    public async Task<Instruction> CreateInstruction(Instruction model)
    {
        Instruction? existingInstruction = _unitOfWork.InstructionRepository.GetByID(model.Id);
        if (existingInstruction != null)
        {
           throw new InstructionNotFoundException();
        }

        await _unitOfWork.InstructionRepository.InsertAsync(model);
        if(await _unitOfWork.SaveAsync() == false)
        {
            _logger.LogError($"Instruction {model.Id} could not be created");
            throw new InstructionCreationException();
        }

        _logger.LogInformation($"Instruction {model.Id} created");
        return model;
    }

    public async Task<InstructionDTO> CreateInstruction(InstructionCreateDTO model)
    {
        return _mapper.Map<InstructionDTO>(await CreateInstruction(_mapper.Map<Instruction>(model)));
    }

    public async Task<bool> DeleteInstructionById(long instructionId)
    {
        Instruction? instruction = _unitOfWork.InstructionRepository.GetByID(instructionId);
        if (instruction == null)
        {
            throw new InstructionNotFoundException();
        }
        _unitOfWork.InstructionRepository.Delete(instruction);
        
        if (await _unitOfWork.SaveAsync() == false)
        {
            _logger.LogError($"Instruction {instructionId} could not be deleted");
            throw new InstructionDeletionException();
        }

        _logger.LogInformation($"Instruction {instructionId} deleted");
        return true;
    }

    public List<InstructionDTO> GetAllInstructions(long userId)
    {
        // TODO: select accessible only
        var instructions = _unitOfWork.InstructionRepository.Get().ToList();
        return _mapper.Map<List<InstructionDTO>>(instructions);
    }

    public List<InstructionDTO> GetCompletedInstructions(long userId)
    {
        var completedTests = _instructionTestService.GetCompletedInstructionTestsByUserId(userId);
        var instructions = _unitOfWork.InstructionRepository.Get()
          .Where(i => completedTests.Any(t => i.InstructionTestId == t.Id));

        return _mapper.Map<List<InstructionDTO>>(instructions);  
    }

    public List<InstructionDTO> GetInstructionById(long userId, long instructionId)
    {
        // TODO: select accessible only
        var instruction = _unitOfWork.InstructionRepository.GetByID(instructionId);
        return _mapper.Map<List<InstructionDTO>>(instruction);
    }

    public List<InstructionDTO> GetInstructionsByCategoryId(long userId, long categoryId)
    {
        var category = _unitOfWork.InstructionCategoryRepository.GetByID(categoryId);
        if (category == null)
        {
            throw new CategoryNotFoundException();
        }

        var instructions = _unitOfWork.InstructionRepository.Get().Where(i => i.CategoryId == categoryId);
        return _mapper.Map<List<InstructionDTO>>(instructions);
    }

    public List<InstructionDTO> GetUnfinishedInstructions(long userId)
    {
        // TODO: only show accessible
        var completedTests = _instructionTestService.GetCompletedInstructionTestsByUserId(userId);

        var instructions = _unitOfWork.InstructionRepository.Get()
          .Where(i => !completedTests.Any(t => i.InstructionTestId == t.Id));

        return _mapper.Map<List<InstructionDTO>>(instructions);
    }

    public async Task<bool> UpdateInstructionById(Instruction model)
    {
        var existingInstruction = _unitOfWork.InstructionRepository.GetByID(model.Id);
        if (existingInstruction == null)
        {
            throw new InstructionNotFoundException();          
        }

        _unitOfWork.InstructionRepository.Update(model);
        if (await _unitOfWork.SaveAsync() == false)
        {
            _logger.LogError($"Instruction {model.Id} could not be updated");
            throw new InstructionUpdateException();
        }
        _logger.LogInformation($"Instruction {model.Id} updated");
        return true;
    }

    public async Task<bool> UpdateInstructionById(InstructionCreateDTO model)
    {
        return await UpdateInstructionById(_mapper.Map<Instruction>(model));
    }
}
