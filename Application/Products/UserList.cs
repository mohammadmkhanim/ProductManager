using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Core;
using Application.Dtos;
using AutoMapper;
using Core.Entities;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Products
{
    public class UserList
    {
        public class Query : IRequest<Result<List<ProductDto>>>
        {
            public string UserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ProductDto>>>
        {
            private readonly ProductManagerContext _context;
            private readonly IMapper _mapper;
            private readonly UserManager<User> _userManager;

            public Handler(ProductManagerContext context, IMapper mapper, UserManager<User> userManager)
            {
                _mapper = mapper;
                _context = context;
                _userManager = userManager;
            }

            public async Task<Result<List<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user is null)
                {
                    return Result<List<ProductDto>>.Failure((int)HttpStatusCode.BadRequest, "The user does not exist.");
                }
                var products = await _context.Products.Where(p => p.UserId == request.UserId).ToListAsync();
                var productDtos = _mapper.Map<List<ProductDto>>(products);
                return Result<List<ProductDto>>.Success((int)HttpStatusCode.OK, productDtos);
            }
        }
    }
}