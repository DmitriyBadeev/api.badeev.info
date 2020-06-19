using Microsoft.AspNetCore.Http;

namespace Portfolio.Storage.Models
{
    public class FileLoadRequestModel
    {
        public IFormFile File { get; set; }

        public string Path { get; set; }
    }
}
