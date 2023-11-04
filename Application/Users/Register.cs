using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Core;
using AutoMapper;
using Core.Entities;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users
{
    public class Register
    {
        public class Command : IRequest<Result<bool>>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<bool>>
        {
            private readonly ProductManagerContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;

            public Handler(ProductManagerContext context, UserManager<User> userManager, IMapper mapper)
            {
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<User>(request);
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    return Result<bool>.Success((int)HttpStatusCode.OK, result.Succeeded);
                }
                else
                {
                    return Result<bool>.Failure((int)HttpStatusCode.BadRequest, result.Errors.FirstOrDefault().Description.ToString());
                }
            }
        }
    }
}