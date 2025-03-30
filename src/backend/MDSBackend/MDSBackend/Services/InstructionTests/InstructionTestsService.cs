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

        // Adding questions using InsertRange
        List<InstructionTestQuestion> newQuestions = _mapper.Map<List<InstructionTestQuestion>>(instructionTest.Questions);
        newQuestions.ForEach(q => q.InstructionTestId = newInstructionTest.Id);
        _unitOfWork.InstructionTestQuestionRepository.InsertRange(newQuestions);

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

    public List<InstructionTestResultDTO> GetUserInstructionTestResultsByInstructionTestId(long userId, long instructionTestId)
    {
        var userTestResults = _unitOfWork.InstructionTestResultRepository.Get(
            q => q.UserId == userId && q.InstructionTestId == instructionTestId).ToList();
        return _mapper.Map<List<InstructionTestResultDTO>>(userTestResults);
    }

    public List<InstructionTestResultDTO> GetInstructionTestResultsByUserId(long userId)
    {
        var userTestResults = _unitOfWork.InstructionTestResultRepository.Get(
            q => q.UserId == userId).ToList();
        return _mapper.Map<List<InstructionTestResultDTO>>(userTestResults);
    }

    public List<InstructionTestDTO> GetInstructionTestsByInstructionId(long instructionId)
    {
        // TODO: InstructionService must be implemented first
        throw new NotImplementedException();
    }

    public async Task<InstructionTestResultDTO> SubmitInstructionTestAsync(long userId, InstructionTestSubmissionDTO submission)
    {
        // Retrieve the test and questions 
        var instructionTest = _unitOfWork.InstructionTestRepository.GetByID(submission.InstructionTestId);
        
        if (instructionTest == null)
        {
            _logger.LogError("Instruction test with id {Id} not found", submission.InstructionTestId);
            throw new InstructionTestNotFoundException();
        }
        
        // Check remaining attempts
        var userTestAttempts = _unitOfWork.InstructionTestResultRepository.Get(
            q => q.UserId == userId && q.InstructionTestId == submission.InstructionTestId).ToList();

        if (userTestAttempts.Count >= instructionTest.MaxAttempts)
        {
            _logger.LogWarning("User {UserId}: denied submission for test {InstructionTestId}: max attempts reached", userId, submission.InstructionTestId);
            throw new InstructionTestSubmissionException();
        }

        var questions = _unitOfWork.InstructionTestQuestionRepository.Get(q => q.InstructionTestId == submission.InstructionTestId).ToList();

        // Verify answers amount
        if (questions.Count != submission.Answers.Count)
        {
            _logger.LogWarning("User {UserId}: denied submission for test {InstructionTestId}: wrong number of answers", userId, submission.InstructionTestId);
            throw new InstructionTestSubmissionException();
        }

        // Evaluate answers
        double score = 0;
        int maxErrorPerQuestion = 1;
        for (int i = 0; i < questions.Count; i++)
        {
            var question = questions[i];

            // User answers for the question without duplicate options
            var answer = submission.Answers[i].Distinct();

            if (question.IsMultipleAnswer)
            {
                int correctUserAnswersCount = 0;
                int incorrectUserAnswersCount = 0;
                int correctAnswersCount = question.CorrectAnswers.Count;

                foreach (var option in answer)
                {
                    if (question.CorrectAnswers.Contains(option))
                    {
                        correctUserAnswersCount++;
                    }
                    else
                    {
                        incorrectUserAnswersCount++;
                    }
                }

                if (incorrectUserAnswersCount > maxErrorPerQuestion || correctUserAnswersCount == 0)
                {
                    // Nothing scored for the question
                    continue;
                }

                // One question is worth 1 point max
                double questionScore = correctUserAnswersCount / (double)correctAnswersCount;

                // Add the question score, or half of it if an error is present
                score += incorrectUserAnswersCount > 0 ? questionScore /= 2 : questionScore;
            }
            else
            {
                score += question.CorrectAnswers.Contains(answer.First()) ? 1 : 0;
            }
        }

        score = Math.Round(score / questions.Count)*100;

        // Add test result
        await _unitOfWork.BeginTransactionAsync();

        InstructionTestResult newTestResult = new InstructionTestResult()
        {
            UserId = userId,
            InstructionTestId = submission.InstructionTestId,
            Score = (int)score
        };

        _unitOfWork.InstructionTestResultRepository.Insert(newTestResult);
        
        if (!await _unitOfWork.SaveAsync())
        {
            _logger.LogError("Failed to save test result for user {UserId} and test {InstructionTestId}", userId, submission.InstructionTestId);
            throw new InstructionTestSubmissionException(); 
        }

        await _unitOfWork.CommitAsync();
        return _mapper.Map<InstructionTestResultDTO>(newTestResult);
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

