using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Models
{

    public class ResponseModel : ActionResult
    {
        private readonly int statusCode;
        private readonly object? value;
        private readonly string? message;

        public ResponseModel(int statusCode, object? value = null, string? message = null)
        {
            this.statusCode = statusCode;
            this.value = value;
            this.message = message;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            var responseObj = new
            {
                value = value,
                message = message
            };
            var jsonResponse = JsonConvert.SerializeObject(responseObj);
            await response.WriteAsync(jsonResponse);
        }
    }
}