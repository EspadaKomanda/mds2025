using AutoMapper;
using MDSBackend.Database.Repositories;
using MDSBackend.Exceptions.Services.InstructionTest;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Services.InstructionTests;

public class InstructionTestsService : IInstructionTestsService
{
    private readonly ILogger<InstructionTestsService> _logger;
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InstructionTestsService(ILogger<InstructionTestsService> logger, UnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // XXX: Make transactional
    public async Task<InstructionTest> CreateInstructionTestAsync(InstructionTestCreateDTO instructionTest)
    {
        // Instruction test creation
        InstructionTest newInstructionTest = _mapper.Map<InstructionTest>(instructionTest);
        await _unitOfWork.InstructionTestRepository.InsertAsync(newInstructionTest);

        if (!await _unitOfWork.SaveAsync())
        {
            throw new InstructionTestCreationException("Failed to save instruction test");
        }

        ICollection<Task> addQuestionTasks = new List<Task>();
        // Adding questions
        foreach (var question in instructionTest.Questions)
        {
            InstructionTestQuestion newQuestion = _mapper.Map<InstructionTestQuestion>(question);
            newQuestion.InstructionTestId = newInstructionTest.Id;
            addQuestionTasks.Add(_unitOfWork.InstructionTestQuestionRepository.InsertAsync(newQuestion));
        }
        await Task.WhenAll(addQuestionTasks);
        

        if (await _unitOfWork.SaveAsync())
        {
            _logger.LogInformation("Instruction test created ({Id})", newInstructionTest.Id);
        }

        throw new InstructionTestCreationException($"Failed to add questions to instruction test {newInstructionTest.Id}");
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
        // TODO: InstructionService must be implemented first
        throw new NotImplementedException();
    }

    public Task<bool> UpdateInstructionTestAsync(InstructionTestCreateDTO instructionTest)
    {
        throw new NotImplementedException();
    }
}

