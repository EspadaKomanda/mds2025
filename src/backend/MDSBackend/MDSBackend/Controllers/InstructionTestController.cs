using AutoMapper;
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
}
