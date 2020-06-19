using Microsoft.AspNetCore.Http;

namespace Portfolio.API.Mutations.InputTypes.File
{
    public class LoadFileInput
    {
        public IFormFile File { get; set; }

        public string Path { get; set; }
    }
}
