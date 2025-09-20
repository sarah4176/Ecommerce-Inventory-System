namespace Ecommerce.Middleware.Common
{
    public enum ErrorType
    {
        // 400 Bad Request
        ValidationError = 400,
        InvalidInput = 400,
        MissingRequiredField = 400,

        // 401 Unauthorized
        Unauthorized = 401,
        InvalidCredentials = 401,
        TokenExpired = 401,

        // 403 Forbidden
        Forbidden = 403,
        InsufficientPermissions = 403,

        // 404 Not Found
        NotFound = 404,
        ResourceNotFound = 404,
        UserNotFound = 404,
        CategoryNotFound = 404,
        ProductNotFound = 404,

        // 409 Conflict
        Conflict = 409,
        AlreadyExists = 409,
        DuplicateEntry = 409,
        CategoryAlreadyExists = 409,
        ProductAlreadyExists = 409,

        // 500 Internal Server Error
        InternalError = 500,
        DatabaseError = 500,
        ExternalServiceError = 500
    }
}
