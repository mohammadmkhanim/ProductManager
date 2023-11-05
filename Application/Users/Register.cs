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
        public class Command : IRequest<Result<Unit>>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
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

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<User>(request);
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    return Result<Unit>.Success((int)HttpStatusCode.Created, Unit.Value);
                }
                else
                {
                    return Result<Unit>.Failure((int)HttpStatusCode.BadRequest, result.Errors.FirstOrDefault().Description.ToString());
                }
            }
        }
    }
}