using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Storage.Models;

namespace Portfolio.Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IHostingEnvironment _env;

        public FileController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> LoadFile([FromForm] FileLoadRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var path = Path.Combine(_env.WebRootPath, model.Path, model.File.FileName);
            Directory.CreateDirectory(Path.Combine(_env.WebRootPath, model.Path));

            try
            {
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    await model.File.CopyToAsync(stream);
                }
            }
            catch (Exception e)
            {
                return Conflict(e);
            }

            var url = Path.Combine(model.Path, model.File.FileName);
            return Ok(url);
        }
    }
}