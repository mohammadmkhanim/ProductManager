using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Core;
using Application.Services;
using AutoMapper;
using Core.Entities;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Users
{
    public class Login
    {
        public class Command : IRequest<Result<string>>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<string>>
        {
            private readonly ProductManagerContext _context;
            private readonly UserManager<User> _userManager;
            IConfiguration _configuration;

            public Handler(ProductManagerContext context, UserManager<User> userManager, IConfiguration configuration)
            {
                _context = context;
                _userManager = userManager;
                _configuration = configuration;
            }


            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
                if (user is null)
                {
                    return Result<string>.Failure((int)HttpStatusCode.NotFound, "The username or password is wrong.");
                }
                if (await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    var token = TokenService.CreateJWTToken(user, _configuration);
                    return Result<string>.Success((int)HttpStatusCode.OK, token);
                }
                else
                {
                    return Result<string>.Failure((int)HttpStatusCode.NotFound, "The username or password is wrong.");
                }
            }
        }
    }
}