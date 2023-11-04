using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }

        public Result(int statusCode)
        {
            StatusCode = statusCode;
        }

        public static Result<T> Success(int statusCode, T value)
        {
            return new Result<T>(statusCode) { IsSuccess = true, Value = value };
        }

        public static Result<T> Failure(int statusCode, string error)
        {
            return new Result<T>(statusCode) { IsSuccess = false, Error = error };
        }
    }
}