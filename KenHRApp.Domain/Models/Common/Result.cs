using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Models.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public T? Value { get; set; }
        public string? Error { get; set; }
        public Dictionary<string, string[]> ValidationErrors { get; set; } = new();

        public static Result<T> SuccessResult(T value) =>
            new Result<T> { Success = true, Value = value };

        public static Result<T> Failure(string error) =>
            new Result<T> { Success = false, Error = error };

        public static Result<T> ValidationFailure(Dictionary<string, string[]> errors) =>
            new Result<T> { Success = false, ValidationErrors = errors };
    }
}
