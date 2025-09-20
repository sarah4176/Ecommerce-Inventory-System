namespace Ecommerce.Middleware.Common
{
    public static class ErrorTypeExtensions
    {
        private static readonly Dictionary<ErrorType, string> ErrorTypeToDefaultMessage = new()
        {
            { ErrorType.ValidationError, "One or more validation errors occurred" },
            { ErrorType.InvalidInput, "Invalid input provided" },
            { ErrorType.MissingRequiredField, "Required field is missing" },

            { ErrorType.Unauthorized, "Unauthorized access" },
            { ErrorType.InvalidCredentials, "Invalid credentials" },
            { ErrorType.TokenExpired, "Token has expired" },

            { ErrorType.Forbidden, "Access forbidden" },
            { ErrorType.InsufficientPermissions, "Insufficient permissions" },

            { ErrorType.NotFound, "Resource not found" },
            { ErrorType.ResourceNotFound, "The requested resource was not found" },
            { ErrorType.UserNotFound, "User not found" },
            { ErrorType.CategoryNotFound, "Category not found" },
            { ErrorType.ProductNotFound, "Product not found" },

            { ErrorType.Conflict, "A conflict occurred" },
            { ErrorType.AlreadyExists, "Resource already exists" },
            { ErrorType.DuplicateEntry, "Duplicate entry detected" },
            { ErrorType.CategoryAlreadyExists, "Category already exists" },
            { ErrorType.ProductAlreadyExists, "Product already exists" },

            { ErrorType.InternalError, "An internal server error occurred" },
            { ErrorType.DatabaseError, "A database error occurred" },
            { ErrorType.ExternalServiceError, "An external service error occurred" }
        };

        public static int GetStatusCode(this ErrorType errorType)
        {
            return (int)errorType;
        }

        public static string GetDefaultMessage(this ErrorType errorType)
        {
            return ErrorTypeToDefaultMessage.TryGetValue(errorType, out var message)
                ? message
                : "An unexpected error occurred";
        }

        public static string GetErrorCode(this ErrorType errorType)
        {
            return errorType.ToString();
        }
    }
}
