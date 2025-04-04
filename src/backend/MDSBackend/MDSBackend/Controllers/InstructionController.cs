using MDSBackend.Exceptions.Services.Instruction;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;
using MDSBackend.Services.Instructions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MDSBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "User")]
public class InstructionController : ControllerBase 
{
    private readonly IInstructionService _instructionService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<InstructionController> _logger;

    public InstructionController(IInstructionService instructionService, UserManager<ApplicationUser> userManager, ILogger<InstructionController> logger)
    {
      _instructionService = instructionService;
      _userManager = userManager;
      _logger = logger;
    }

    /// <summary>
    /// Create a new instruction.
    /// </summary>
    /// <param name="model">The instruction model.</param>
    /// <returns><see cref="InstructionDTO"/> which was created</returns>
    /// <response code="200">Returns the created instruction</response>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> CreateInstruction([FromBody] InstructionCreateDTO model) 
    {
        model.Id = 0;
        var instruction = await _instructionService.CreateInstruction(model);
        return Ok(instruction);
    }

    /// <summary>
    /// Update an existing instruction.
    /// </summary>
    /// <param name="model">The instruction model. Id must match the object which is being updated.</param>
    /// <returns><see cref="bool"/></returns>
    /// <response code="200"></response>
    /// <response code="404">If the instruction is not found</response>
    [HttpPut]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateInstruction([FromBody] InstructionCreateDTO model) 
    {
        var instruction = await _instructionService.UpdateInstructionById(model);
        return Ok(instruction);
    }

    /// <summary>
    /// Delete an existing instruction.
    /// </summary>
    /// <param name="id">The ID of the instruction to delete.</param>
    /// <returns><see cref="bool"/></returns>
    /// <response code="200"></response>
    /// <response code="404">If the instruction is not found</response>    
    [HttpDelete]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteInstruction(long id)
    {
        try
        {
            return Ok(await _instructionService.DeleteInstructionById(id));
        }
        catch (InstructionNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Retrieve all instructions for the authenticated user.
    /// </summary>
    /// <returns>A list of <see cref="InstructionDTO"/> for the user.</returns>
    /// <response code="200">Returns the list of all instructions</response>
    [HttpGet("all")]
    public async Task<IActionResult> GetAllInstructions()
    {
        string username = User.Claims.First(c => c.Type == "username").Value;
        long userId = (await _userManager.FindByNameAsync(username))!.Id;

        return Ok(_instructionService.GetAllInstructions(userId));
    }

    /// <summary>
    /// Retrieve all completed instructions for the authenticated user.
    /// </summary>
    /// <returns>A list of <see cref="InstructionDTO"/> that are completed for the user.</returns>
    /// <response code="200">Returns the list of completed instructions</response>
    [HttpGet("completed")]
    public async Task<IActionResult> GetCompletedInstructions()
    {
        string username = User.Claims.First(c => c.Type == "username").Value;
        long userId = (await _userManager.FindByNameAsync(username))!.Id;

        return Ok(_instructionService.GetCompletedInstructions(userId));
    }

    /// <summary>
    /// Retrieve all unfinished instructions for the authenticated user.
    /// </summary>
    /// <returns>A list of <see cref="InstructionDTO"/> that are unfinished for the user.</returns>
    /// <response code="200">Returns the list of unfinished instructions</response>
    [HttpGet("unfinished")]
    public async Task<IActionResult> GetUnfinishedInstructions()
    {
        string username = User.Claims.First(c => c.Type == "username").Value;
        long userId = (await _userManager.FindByNameAsync(username))!.Id;

        return Ok(_instructionService.GetUnfinishedInstructions(userId));
    }

    /// <summary>
    /// Retrieve instructions by category ID for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the category to filter instructions.</param>
    /// <returns>A list of <see cref="InstructionDTO"/> for the specified category.</returns>
    /// <response code="200">Returns the list of instructions for the specified category</response>
    /// <response code="404">If the category is not found</response>
    [HttpGet("category/{id}")]
    public async Task<IActionResult> GetInstructionsByCategoryId(long id)
    {
        try
        {
            string username = User.Claims.First(c => c.Type == "username").Value;
            long userId = (await _userManager.FindByNameAsync(username))!.Id;

            return Ok(_instructionService.GetInstructionsByCategoryId(userId, id));
        }
        catch (CategoryNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Retrieve a specific instruction by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the instruction to retrieve.</param>
    /// <returns><see cref="InstructionDTO"/> for the specified instruction.</returns>
    /// <response code="200">Returns the instruction with the specified ID</response>
    /// <response code="404">If the instruction is not found</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInstructionById(long id)
    {
        try 
        {
            string username = User.Claims.First(c => c.Type == "username").Value;
            long userId = (await _userManager.FindByNameAsync(username))!.Id;

            return Ok(_instructionService.GetInstructionById(userId, id));
        }
        catch(InstructionNotFoundException)
        {
            return NotFound();
        }
    }
}

