using API.Models;
using Application.Products;
using Application.Users;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseController<ProductsController>
{
    public ProductsController(IMediator mediator, ILogger<ProductsController> logger, IConfiguration configuration) : base(mediator, logger: logger, configuration: configuration)
    {
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult> GetAsync([FromRoute] string userId)
    {
        return HandleResult(await _mediator.Send(new UserList.Query() { UserId = userId }));
    }

    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        return HandleResult(await _mediator.Send(new List.Query()));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] Create.Command command)
    {
        return HandleResult(await _mediator.Send(command));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        return HandleResult(await _mediator.Send(new Delete.Command() { Id = id }));
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> EditAsync([FromBody] Edit.Command command)
    {
        return HandleResult(await _mediator.Send(command));
    }
}
