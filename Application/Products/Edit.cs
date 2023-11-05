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
    public class Edit
    {
        public class Command : IRequest<Result<ProductDto>>
        {
            [Key]
            public int Id { get; set; }

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
                    return Result<ProductDto>.Failure((int)HttpStatusCode.Forbidden, "The user who wants to edit a product must have created it.");
                }
                if (await _context.Products.AnyAsync(p => product.Id != request.Id && p.ManufactureEmail == product.ManufactureEmail))
                {
                    return Result<ProductDto>.Failure((int)HttpStatusCode.BadRequest, "The manufacture email has already exist.");
                }
                if (await _context.Products.AnyAsync(p => product.Id != request.Id && p.ProduceDate == product.ProduceDate))
                {
                    return Result<ProductDto>.Failure((int)HttpStatusCode.BadRequest, "The produce date has already exist.");
                }
                var entry = _context.Entry(product);
                if (entry.State != EntityState.Detached)
                {
                    entry.State = EntityState.Detached;
                }
                product = _mapper.Map<Product>(request);
                product.UserId = userId;
                _context.Products.Update(product);
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