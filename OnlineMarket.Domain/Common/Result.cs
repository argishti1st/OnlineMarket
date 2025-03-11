using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMarket.Domain.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }
        public ResultStatus Status { get; }

        private Result(T value, ResultStatus status, bool isSuccess = true)
        {
            Value = value;
            Status = status;
            IsSuccess = isSuccess;
        }

        private Result(string errorMessage, ResultStatus status)
        {
            ErrorMessage = errorMessage;
            Status = status;
            IsSuccess = false;
        }

        public static Result<T> Failure(T value, bool isSuccess) => new(value, ResultStatus.BadRequest, isSuccess);

        public static Result<T> Success(T value) => new(value, ResultStatus.Success);

        public static Result<T> NotFound(string message) => new(message, ResultStatus.NotFound);

        public static Result<T> BadRequest(string message) => new(message, ResultStatus.BadRequest);

        public static Result<T> Forbidden(string message) => new(message, ResultStatus.Forbidden);
    }
}
