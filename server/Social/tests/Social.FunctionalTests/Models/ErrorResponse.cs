namespace Social.FunctionalTests.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }

        public ErrorResponse()
        {
            Errors = new Dictionary<string, List<string>>();
        }
    }
}
