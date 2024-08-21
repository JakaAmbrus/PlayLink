namespace Social.Api.Errors
{
    public class ApiException
    {
        public ApiException(int statusCode, string message = null, string details = null, object errors = null)
        {
            StatusCode = statusCode;
            Message = message ?? "An error occurred.";
            Details = details;
            Errors = errors;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public object Errors { get; set; }
    }
}

