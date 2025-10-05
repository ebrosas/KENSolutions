using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class RepositoryResult<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public static RepositoryResult<T> Ok(T data) => new RepositoryResult<T> { Success = true, Data = data };

        public static RepositoryResult<T> Fail(string errorMessage) => new RepositoryResult<T> { Success = false, ErrorMessage = errorMessage };
    }
}
