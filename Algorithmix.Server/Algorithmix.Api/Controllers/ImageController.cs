using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Algorithmix.Server.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public ImageController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetImage(string src)
        {
            var path = Path.Combine(_env.WebRootPath, src);
            var image = System.IO.File.OpenRead(path);

            return Ok(image);
        }
    }
}
