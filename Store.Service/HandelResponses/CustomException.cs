namespace Store.Service.HandelResponses
{
    public class CustomException : Response
    {
        public CustomException(int statusCode, string? statusMessage = null,string? details = null) : base(statusCode, statusMessage)
        {
            Details = details;
        }
        public string? Details { get; set; }
    }
}
