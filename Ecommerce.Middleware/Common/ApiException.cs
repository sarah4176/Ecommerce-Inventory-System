namespace Ecommerce.Middleware.Common
{
    public class ApiException : Exception
    {
        public ErrorType ErrorType { get; }
        public int StatusCode => ErrorType.GetStatusCode();
        public string ErrorCode => ErrorType.GetErrorCode();

        public ApiException(ErrorType errorType, string message = null)
            : base(message ?? errorType.GetDefaultMessage())
        {
            ErrorType = errorType;
        }

        public ApiException(ErrorType errorType, string message, Exception innerException)
            : base(message ?? errorType.GetDefaultMessage(), innerException)
        {
            ErrorType = errorType;
        }
        public static ApiException NotFound(string message = null)
            => new(ErrorType.NotFound, message);
        public static ApiException BadRequest(string message = null)
            => new(ErrorType.BadRequest, message);
        public static ApiException CategoryNotFound(int id)
            => new(ErrorType.CategoryNotFound, $"Category with ID {id} not found");

        public static ApiException ProductNotFound(int id)
            => new(ErrorType.ProductNotFound, $"Product with ID {id} not found");

        public static ApiException UserNotFound(string identifier)
            => new(ErrorType.UserNotFound, $"User with identifier {identifier} not found");

        public static ApiException Conflict(string message = null)
            => new(ErrorType.Conflict, message);

        public static ApiException CategoryAlreadyExists(string name)
            => new(ErrorType.CategoryAlreadyExists, $"Category with name '{name}' already exists");

        public static ApiException ProductAlreadyExists(string name)
            => new(ErrorType.ProductAlreadyExists, $"Product with name '{name}' already exists");

        public static ApiException Validation(string message = null)
            => new(ErrorType.ValidationError, message);

        public static ApiException Unauthorized(string message = null)
            => new(ErrorType.Unauthorized, message);

        public static ApiException Forbidden(string message = null)
            => new(ErrorType.Forbidden, message);
    }
}
