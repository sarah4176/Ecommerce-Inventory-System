namespace Ecommerce.Middleware.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public int StatusCode { get; set; }

        public static ApiResponse SuccessResponse(object data, string message = "Success", int statusCode = 200)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ApiResponse ErrorResponse(string message, int statusCode = 400)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Data = null,
                StatusCode = statusCode
            };
        }
    }
}
