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
    public class Create
    {
        public class Command : IRequest<Result<ProductDto>>
        {
            [Required(ErrorMessage = "{0} is requierd.")]
            [MaxLength(40, ErrorMessage = "Max length of {0} should be {1}.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "{0} is requierd.")]
            public DateTime ProduceDate { get; set; }

            [Required(ErrorMessage = "{0} is requierd.")]
            [RegularExpression(@"^09\d{9}$", ErrorMessage = "Enter the phone correctly.")]
            public string ManufacturePhone { get; set; }

            [Required(ErrorMessage = "{0} is requierd.")]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter the email correctly.")]
            public string ManufactureEmail { get; set; }

            [Required(ErrorMessage = "{0} is requierd.")]
            public bool IsAvailable { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<ProductDto>>
        {
            private readonly ProductManagerContext _context;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(ProductManagerContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
            {
                _mapper = mapper;
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Result<ProductDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = _mapper.Map<Product>(request);
                if (await _context.Products.AnyAsync(p => p.ManufactureEmail == product.ManufactureEmail))
                {
                    return Result<ProductDto>.Failure((int)HttpStatusCode.BadRequest, "The manufacture email has already exist.");
                }
                if (await _context.Products.AnyAsync(p => p.ProduceDate == product.ProduceDate))
                {
                    return Result<ProductDto>.Failure((int)HttpStatusCode.BadRequest, "The produce date has already exist.");
                }
                var user = _httpContextAccessor.HttpContext.User;
                product.UserId = user.FindFirst("Id").Value;
                await _context.Products.AddAsync(product);
                var result = await _context.SaveChangesAsync();
                var productDto = _mapper.Map<ProductDto>(product);
                if (result > 0)
                {
                    return Result<ProductDto>.Success((int)HttpStatusCode.Created, productDto);
                }
                else
                {
                    return Result<ProductDto>.Failure((int)HttpStatusCode.InternalServerError, "Internal server error.");
                }
            }
        }

    }
}