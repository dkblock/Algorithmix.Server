using Algorithmix.Api.Core;
using Microsoft.AspNetCore.Mvc;

namespace Algorithmix.Server.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageController : Controller
    {
        private readonly TestDataManager _testDataManager;

        public ImageController(TestDataManager testDataManager)
        {
            _testDataManager = testDataManager;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetImage(string src)
        {
            if (!_testDataManager.Exists(src))
                return NotFound();

            var image = _testDataManager.GetImage(src);
            return Ok(image);
        }
    }
}
