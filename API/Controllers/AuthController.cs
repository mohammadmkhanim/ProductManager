using API.Models;
using Application.Users;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController : BaseController<AuthController>
{
    private readonly UserManager<IdentityUser> _userManager;
    public AuthController(IMediator mediator, ILogger<AuthController> logger, IConfiguration configuration) : base(mediator, logger: logger, configuration: configuration)
    {
    }

    [HttpPost]
    public async Task<ActionResult> RegisterAsync([FromBody] Register.Command command)
    {
        return HandleResult(await _mediator.Send(command));
    }

    [HttpPost]
    public async Task<ActionResult> LoginAsync([FromBody] Login.Command command)
    {
        return HandleResult(await _mediator.Send(command));
    }
}
