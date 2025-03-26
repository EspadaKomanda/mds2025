using AutoMapper;
using MDSBackend.Exceptions.Services.InstructionTest;
using MDSBackend.Models.DTO;
using MDSBackend.Services.InstructionTests;
using Microsoft.AspNetCore.Mvc;

namespace MDSBackend.Controllers;

public class InstructionTestController : Controller
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
    public IActionResult GetInstructionTestById(int id)
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
    public IActionResult GetInstructionTestByInstructionId(int instructionId)
    {
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
    public IActionResult GetInstructionTestQuestionsByInstructionTestId(int instructionTestId)
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
}
