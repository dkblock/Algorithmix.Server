using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Server.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly ApplicationContext _db;

        public UsersController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<ApplicationUserEntity> Get()
        {
            var users = _db.ApplicationUsers.ToList();
            return new List<ApplicationUserEntity>();

        }
    }
}
