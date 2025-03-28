using AutoMapper;
using MDSBackend.Exceptions.Services.InstructionTest;
using MDSBackend.Models.DTO;
using MDSBackend.Services.InstructionTests;
using Microsoft.AspNetCore.Mvc;

namespace MDSBackend.Controllers;

[ApiController]
public class InstructionTestController : ControllerBase
{
    private readonly IInstructionTestsService _instructionTestsService;
    private readonly ILogger<InstructionTestController> _logger;
    private readonly IMapper _mapper;

    public InstructionTestController(IInstructionTestsService instructionTestsService, ILogger<InstructionTestController> logger, IMapper mapper)
    {
        _instructionTestsService = instructionTestsService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets an instruction test by its ID.
    /// </summary>
    /// <param name="id">The ID of the instruction test.</param>
    /// <returns>An <see cref="InstructionTestDTO"/> containing the instruction test DTO if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test DTO</response>
    /// <response code="404">If the instruction test is not found</response>
    [HttpGet]
    public IActionResult GetInstructionTestById(long id)
    {
        try
        {
            var instructionTest = _instructionTestsService.GetInstructionTestById(id);
            return Ok(_mapper.Map<InstructionTestDTO>(instructionTest));
        }
        catch (InstructionTestNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Gets an instruction test by its instruction ID.
    /// </summary>
    /// <param name="instructionId">The ID of the instruction.</param>
    /// <returns>An <see cref="InstructionTestDTO"/> containing the instruction test DTO if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test DTO</response>
    /// <response code="404">If the instruction test is not found</response>
    [HttpGet]
    public IActionResult GetInstructionTestByInstructionId(long instructionId)
    {
        // WARNING: The service method is not implemented at the time of writing this
        try
        {
            var instructionTest = _instructionTestsService.GetInstructionTestsByInstructionId(instructionId);
            return Ok(_mapper.Map<InstructionTestDTO>(instructionTest));
        }
        catch (InstructionTestNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Gets all instruction test questions by instruction test ID.
    /// </summary>
    /// <param name="instructionTestId">The ID of the instruction test.</param>
    /// <returns>A list of <see cref="InstructionTestQuestionDTO"/> containing the instruction test questions if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test questions</response>
    /// <response code="404">If the instruction test questions are not found</response>
    [HttpGet]
    public IActionResult GetInstructionTestQuestionsByInstructionTestId(long instructionTestId)
    {
        try
        {
            var instructionTestQuestions = _instructionTestsService.GetInstructionTestQuestionsByInstructionTestId(instructionTestId);
            return Ok(instructionTestQuestions);
        }
        catch (InstructionTestNotFoundException)
        {
            return NotFound();
        }
    } 

    /// <summary>
    /// Gets all instruction test results for a user by instruction ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="instructionId">The ID of the instruction.</param>
    /// <returns>A list of <see cref="InstructionTestResultDTO"/> containing the instruction test results if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test results</response>
    /// <response code="404">If the instruction test results are not found</response>
    [HttpGet]
    public IActionResult GetUserInstructionTestResultsByInstructionId(long userId, long instructionTestId)
    {
        try
        {
            var instructionTestResults = _instructionTestsService.GetUserInstructionTestResultsByInstructionTestId(userId, instructionTestId);
            return Ok(instructionTestResults);
        }
        catch (InstructionTestNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Gets all instruction test results for a user by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of <see cref="InstructionTestResultDTO"/> containing the instruction test results if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test results</response>
    /// <response code="404">If the instruction test results are not found</response>
    [HttpGet]
    public IActionResult GetInstructionTestResultsByUserId(long userId)
    {
        try
        {
            var instructionTestResults = _instructionTestsService.GetInstructionTestResultsByUserId(userId);
            return Ok(instructionTestResults);
        }
        catch (InstructionTestNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Gets all completed instruction test results for a user by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of <see cref="InstructionTestResultDTO"/> containing the instruction test results if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test results</response>
    /// <response code="404">If the instruction test results are not found</response>
    [HttpGet]
    public IActionResult GetCompletedInstructionTestsByUserId(long userId)
    {
        try
        {
            var instructionTestResults = _instructionTestsService.GetCompletedInstructionTestsByUserId(userId);
            return Ok(instructionTestResults);
        }
        catch (InstructionTestNotFoundException)
        {
            return NotFound();
        }
    }
}
