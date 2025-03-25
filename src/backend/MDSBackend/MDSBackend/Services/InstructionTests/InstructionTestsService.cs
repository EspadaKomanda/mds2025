using AutoMapper;
using MDSBackend.Database.Repositories;
using MDSBackend.Exceptions.Services.InstructionTest;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;
using MDSBackend.Utils.Enums;

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

    public async Task<InstructionTestDTO> CreateInstructionTestAsync(InstructionTestCreateDTO instructionTest)
    {
        await _unitOfWork.BeginTransactionAsync();
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
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Instruction test created ({Id})", newInstructionTest.Id);
            return _mapper.Map<InstructionTestDTO>(newInstructionTest);
        }

        throw new InstructionTestCreationException($"Failed to add questions to instruction test {newInstructionTest.Id}");
      }

    public async Task<bool> DeleteInstructionTestByIdAsync(long id)
    {
        var instructionTest = _unitOfWork.InstructionTestRepository.GetByID(id);

        if (instructionTest == null)
        {
            _logger.LogError("Instruction test with id {Id} not found", id);
            throw new InstructionTestNotFoundException();
        }

        // Find all questions 
        var questions = _unitOfWork.InstructionTestQuestionRepository.Get(q => q.InstructionTestId == id);

        // Start transaction
        await _unitOfWork.BeginTransactionAsync();

        foreach (var question in questions)
        {
            _unitOfWork.InstructionTestQuestionRepository.Delete(question);
        }

        // Delete instruction test
        _unitOfWork.InstructionTestRepository.Delete(instructionTest);

        if (await _unitOfWork.SaveAsync())
        {
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Instruction test deleted ({Id})", id);
            return true;
        }
        else
        {
            _logger.LogError("Failed to delete instruction test ({Id})", id);
            throw new InstructionTestDeletionException();
        }

    }

    public List<InstructionTestResultDTO> GetCompletedInstructionTestsByUserId(long userId)
    {
        var userTestAttempts = _unitOfWork.InstructionTestResultRepository.Get(
            q => q.UserId == userId).ToList();

        var userInstructionTests = _unitOfWork.InstructionTestRepository.Get(
            q => userTestAttempts.Any(a => a.InstructionTestId == q.Id)).ToList();

        var conclusiveUserTestResults = new List<InstructionTestResultDTO>();
        foreach (var instructionTest in userInstructionTests)
        {
            var scoreCalcMethod = instructionTest.ScoreCalcMethod;
            int maxScore = 0;

            if (scoreCalcMethod == InstructionTestScoreCalcMethod.AverageGrade)
            {
                maxScore = (int)Math.Round(userTestAttempts.Where(q => q.InstructionTestId == instructionTest.Id).Average(q => q.Score));
            }
            else
            {
                maxScore = userTestAttempts.Where(q => q.InstructionTestId == instructionTest.Id).Max(q => q.Score);
            }

            if (maxScore >= instructionTest.MinScore)
            {
                conclusiveUserTestResults.Add(_mapper.Map<InstructionTestResultDTO>(userTestAttempts.First(q => q.InstructionTestId == instructionTest.Id)));
            }
        }

        return conclusiveUserTestResults;
    }

    public InstructionTestDTO GetInstructionTestById(long id)
    {
        var instructionTest = _unitOfWork.InstructionTestRepository.GetByID(id);
        if (instructionTest == null)
        {
            _logger.LogError("Instruction test with id {Id} not found", id);
            throw new InstructionTestNotFoundException();
        }
        return _mapper.Map<InstructionTestDTO>(instructionTest);
    }

    public List<InstructionTestQuestionDTO> GetInstructionTestQuestionsByInstructionTestId(long instructionTestId)
    {
        var questions = _unitOfWork.InstructionTestQuestionRepository.Get(q => q.InstructionTestId == instructionTestId);
        return _mapper.Map<List<InstructionTestQuestionDTO>>(questions);
    }

    public Task<List<InstructionTestResultDTO>> GetInstructionTestResultsByInstructionId(long instructionId)
    {
        throw new NotImplementedException();
    }

    public Task<List<InstructionTestResultDTO>> GetInstructionTestResultsByUserId(long userId)
    {
        throw new NotImplementedException();
    }

    public List<InstructionTestDTO> GetInstructionTestsByInstructionId(long instructionId)
    {
        // TODO: InstructionService must be implemented first
        throw new NotImplementedException();
    }

    public Task<InstructionTestResultDTO> SubmitInstructionTestAsync(long userId, InstructionTestSubmissionDTO submission)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateInstructionTestAsync(InstructionTestCreateDTO instructionTest)
    {
        if (instructionTest == null)
        {
            _logger.LogWarning("Instruction test id was not provided for update.");
            throw new InstructionTestNotFoundException();
        }
        var oldInstructionTest = _unitOfWork.InstructionTestRepository.GetByID(instructionTest.Id!);

        if (oldInstructionTest == null)
        {
            _logger.LogWarning("Instruction test with id {Id} not found for update", instructionTest.Id);
            throw new InstructionTestNotFoundException();
        }

        _mapper.Map(instructionTest, oldInstructionTest);
        _unitOfWork.InstructionTestRepository.Update(oldInstructionTest);
        return await _unitOfWork.SaveAsync();
    }
}

