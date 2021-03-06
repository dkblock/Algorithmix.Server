﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace VisualAlgorithms.Server.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImagesController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public ImagesController(IWebHostEnvironment env)
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
