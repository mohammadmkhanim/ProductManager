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
    public class Delete
    {
        public class Command : IRequest<Result<ProductDto>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<ProductDto>>
        {
            private readonly ProductManagerContext _context;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly UserManager<User> _userManager;

            public Handler(ProductManagerContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
            {
                _mapper = mapper;
                _context = context;
                _httpContextAccessor = httpContextAccessor;
                _userManager = userManager;
            }

            public async Task<Result<ProductDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.FindAsync(request.Id);
                if (product is null)
                {
                    return Result<ProductDto>.Failure((int)HttpStatusCode.BadRequest, "The product does not exist.");
                }
                var userId = _httpContextAccessor.HttpContext.User.FindFirst("Id").Value;
                if (product.UserId != userId)
                {
                    return Result<ProductDto>.Failure((int)HttpStatusCode.Forbidden, "The user who wants to delete a product must have created it.");
                }
                _context.Products.Remove(product);
                var result = await _context.SaveChangesAsync();
                var productDto = _mapper.Map<ProductDto>(product);
                if (result > 0)
                {
                    return Result<ProductDto>.Success((int)HttpStatusCode.OK, productDto);
                }
                else
                {
                    return Result<ProductDto>.Success((int)HttpStatusCode.InternalServerError, productDto);
                }
            }
        }

    }
}