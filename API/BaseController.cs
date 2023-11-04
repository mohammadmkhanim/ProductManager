using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BaseController<ControllerType> : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<ControllerType> _logger;
        protected readonly IMapper _mapper;

        protected BaseController(
            IMediator mediator = null,
            IMapper mapper = null,
            ILogger<ControllerType> logger = null,
            IConfiguration configuration = null)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            return new ResponseModel(result.StatusCode, result.Value, result.Error);
        }


    }
}