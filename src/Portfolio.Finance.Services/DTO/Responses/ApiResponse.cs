using System.Net;

namespace Portfolio.Finance.Services.DTO.Responses
{
    public class ApiResponse
    {
        public string JsonContent { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}