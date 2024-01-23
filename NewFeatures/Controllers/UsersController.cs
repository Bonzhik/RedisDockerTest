using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewFeatures.Data;
using NewFeatures.Models;
using NewFeatures.Services.Cache;

namespace NewFeatures.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly ApiDbContext _db;
        private readonly ICacheService _cacheService;

        public UsersController(ApiDbContext db, ICacheService cacheService)
        {
            _db = db;
            _cacheService = cacheService;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var cacheData = await _cacheService.GetDataAsync<IEnumerable<User>>("users");
            if (cacheData != null)
                return Ok(cacheData);
            cacheData = await _db.Users.ToListAsync();

            var expTime = DateTime.Now.AddMinutes(1);
            await _cacheService.SetDataAsync("users", cacheData, expTime);

            return Ok(cacheData);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok();
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var userExists = await _db.Users.AnyAsync(u => u.Id == user.Id);
            if (userExists)
            {
                return BadRequest();
            }

            _db.Users.Add(user);
            if (!(await _db.SaveChangesAsync() > 0))
            {
                return BadRequest();
            }

            var expTime = DateTime.Now.AddMinutes(1);
            await _cacheService.SetDataAsync("users", _db.Users.ToList(), expTime);
            return Ok(user);
        }
    }

}
