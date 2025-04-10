using AutoMapper;
using MDSBackend.Exceptions.Services.Instruction;
using MDSBackend.Exceptions.Services.InstructionTest;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;
using MDSBackend.Services.InstructionTests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MDSBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "User")]
public class InstructionTestController : ControllerBase
{
    private readonly IInstructionTestsService _instructionTestsService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<InstructionTestController> _logger;
    private readonly IMapper _mapper;

    public InstructionTestController(IInstructionTestsService instructionTestsService, UserManager<ApplicationUser> userManager, ILogger<InstructionTestController> logger, IMapper mapper)
    {
        _instructionTestsService = instructionTestsService;
        _userManager = userManager;
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
    [HttpGet("{id}")]
    public IActionResult GetInstructionTestById(long id)
    {
        // TODO: verify admin access / user ownership
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
    /// <param name="id">The ID of the instruction.</param>
    /// <returns>An <see cref="InstructionTestDTO"/> containing the instruction test DTO if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test DTO</response>
    /// <response code="404">If the instruction is not found</response>
    [HttpGet("instruction/{id}")]
    public IActionResult GetInstructionTestsByInstructionId(long id)
    {
        // TODO: verify admin access / user ownership
        try
        {
            var instructionTest = _instructionTestsService.GetInstructionTestsByInstructionId(id);
            return Ok(_mapper.Map<InstructionTestDTO>(instructionTest));
        }
        catch (InstructionTestNotFoundException)
        {
            return Ok(new List<InstructionTestDTO>());
        }
        catch (InstructionNotFoundException)
        {
            return NotFound("Instruction not found");
        }
    }

    /// <summary>
    /// Gets all instruction test questions by instruction test ID.
    /// </summary>
    /// <param name="instructionTestId">The ID of the instruction test.</param>
    /// <returns>A list of <see cref="InstructionTestQuestionDTO"/> containing the instruction test questions if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test questions</response>
    /// <response code="404">If the instruction test questions are not found</response>
    [HttpGet("{instructionTestId}/questions")]
    public IActionResult GetInstructionTestQuestionsByInstructionTestId(long instructionTestId)
    {
        // TODO: verify admin access / user ownership
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
    /// Gets all instruction test results for authorized user by instruction ID.
    /// </summary>
    /// <param name="instructionTestId">The ID of the instruction.</param>
    /// <returns>A list of <see cref="InstructionTestResultDTO"/> containing the instruction test results if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test results</response>
    /// <response code="404">If the instruction test results are not found</response>
    [HttpGet("/{instructionTestId}/results")]
    public async Task<IActionResult> GetUserInstructionTestResultsByInstructionTestId(long instructionTestId)
    {
        // TODO: verify user ownership
        string username = User.Claims.First(c => c.Type == "username").Value;
        long userId = (await _userManager.FindByNameAsync(username))!.Id;

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
    /// Gets all instruction test results for a specific user by instruction test ID (admin access).
    /// </summary>
    /// <param name="userId">The ID of the user whose results are being requested.</param>
    /// <param name="instructionTestId">The ID of the instruction.</param>
    /// <returns>A list of <see cref="InstructionTestResultDTO"/> containing the instruction test results if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test results</response>
    /// <response code="404">If the instruction test results are not found</response>
    /// <response code="403">If the user is not an admin</response>
    [HttpGet("{instructionTestId}/user/{userId}/results")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetInstructionTestResultsForUserByInstructionTestId(long userId, long instructionTestId)
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
    /// <param name="id">The ID of the user.</param>
    /// <returns>A list of <see cref="InstructionTestResultDTO"/> containing the instruction test results if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test results</response>
    /// <response code="404">If the instruction test results are not found</response>
    [HttpGet("user/{id}/results")]
    public IActionResult GetInstructionTestResultsByUserId(long id)
    {
        // TODO: verify admin access / user ownership
        try
        {
            var instructionTestResults = _instructionTestsService.GetInstructionTestResultsByUserId(id);
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
    /// <param name="id">The ID of the user.</param>
    /// <returns>A list of <see cref="InstructionTestResultDTO"/> containing the instruction test results if found, or a 404 Not Found if not found.</returns>
    /// <response code="200">Returns the instruction test results</response>
    /// <response code="404">If the instruction test results are not found</response>
    [HttpGet("user/{id}/completed")]
    public IActionResult GetCompletedInstructionTestsByUserId(long id)
    {
        // TODO: verify admin access / user ownership
        try
        {
            var instructionTestResults = _instructionTestsService.GetCompletedInstructionTestsByUserId(id);
            return Ok(instructionTestResults);
        }
        catch (InstructionTestNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Creates a new instruction test.
    /// </summary>
    /// <param name="model">The instruction test model.</param>
    /// <returns>A <see cref="InstructionTestDTO"/> containing the created instruction test if successful, or a 500 Internal Server Error if not successful.</returns>
    /// <response code="200">Returns the created instruction test</response>
    [HttpPost]
    public async Task<IActionResult> CreateInstructionTest([FromBody] InstructionTestCreateDTO model)
    {
        try
        {
            var instructionTest = await _instructionTestsService.CreateInstructionTest(model);
            return Ok(instructionTest);
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to create instruction test");
        }
    }

    /// <summary>
    /// Updates an existing instruction test.
    /// </summary>
    /// <param name="model">The instruction test model.</param>
    /// <returns>A <see cref="InstructionTestDTO"/> containing the updated instruction test if successful, or a 500 Internal Server Error if not successful.</returns>
    /// <response code="200">Returns the updated instruction test</response>
    [HttpPut]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateInstructionTest([FromBody] InstructionTestCreateDTO model)
    {
        try
        {
            var instructionTest = await _instructionTestsService.UpdateInstructionTest(model);
            return Ok(instructionTest);
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to update instruction test");
        }
    }

    /// <summary>
    /// Deletes an existing instruction test.
    /// </summary>
    /// <param name="id">The ID of the instruction test to delete.</param>
    /// <returns>A <see cref="bool"/></returns>
    /// <response code="200">Returns the deletion status.</response>
    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteInstructionTest(long id)
    {
        try
        {
            await _instructionTestsService.DeleteInstructionTestByIdAsync(id);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to delete instruction test");
        }
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitInstructionTest([FromBody] InstructionTestSubmissionDTO model)
    {
        // TODO: verify user access
        string username = User.Claims.First(c => c.Type == "username").Value;
        long userId = (await _userManager.FindByNameAsync(username))!.Id;

        try
        {
            await _instructionTestsService.SubmitInstructionTestAsync(userId, model);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to submit instruction test");
        }
    }
}
