namespace Restaurant.Business.Responses
{
    public class AuthorizeErrorResponse
    {
        public AuthorizeErrorResponse() { }

        public AuthorizeErrorResponse(string error)
        {
            Error = error;
        }

        public string Error { get; set; }
    }
}
