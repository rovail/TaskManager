using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Models;
using TaskManagerApi.Models;
using TaskManagerApi.Models.Data;
using TaskManagerApi.Models.Services;

#nullable disable
namespace TaskManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;

        public UsersController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult TestApi()
        {
            return Ok("Сервер запущен, время запуска: " + DateTime.Now);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            if (!IsUserExists(userModel))
            {
                if (userModel != null)
                {
                    bool res = _userService.Create(userModel);
                    return res ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel userModel)
        {
            if(userModel != null)
            {
                bool res = _userService.Update(id, userModel);
                return res ? Ok() : NotFound();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            bool res = _userService.Delete(id);
            return res ? Ok() : NotFound();
        }

        [HttpGet("{id}")]
        public ActionResult<UserModel> GerUser(int id)
        {
            var res = _userService.Get(id);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            return await _db.Users.Select(u => u.ToDto()).ToListAsync();
        }

        [HttpPost("all")]
        public IActionResult CreateMultipleUsers([FromBody] List<UserModel> userModels)
        {
            if (userModels != null && userModels.Count > 0)
            {
                bool res = _userService.CreateMultipleUsers(userModels);
                return res ? Ok() : NotFound();
            }
            return BadRequest();
        }

        private bool IsUserExists(UserModel userModel)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == userModel.Email);

            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}
