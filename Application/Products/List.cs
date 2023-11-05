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
    public class List
    {
        public class Query : IRequest<Result<List<ProductDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<ProductDto>>>
        {
            private readonly ProductManagerContext _context;
            private readonly IMapper _mapper;

            public Handler(ProductManagerContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<List<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _context.Products.ToListAsync();
                var productDtos = _mapper.Map<List<ProductDto>>(products);
                return Result<List<ProductDto>>.Success((int)HttpStatusCode.OK, productDtos);
            }
        }
    }
}