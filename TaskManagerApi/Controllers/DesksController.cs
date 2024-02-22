using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Models;
using TaskManagerApi.Models.Data;
using TaskManagerApi.Models.Services;

#nullable disable
namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesksController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;
        private readonly DeskService _deskService;

        public DesksController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
            _deskService = new DeskService(db);
        }

        [HttpGet]
        public async Task<IEnumerable<object>> GetDeskForUser()
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if(user != null) 
            {
                return await _deskService.GetAll(user.Id).ToListAsync();
            }
            return Array.Empty<CommonModel>();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var desk = _deskService.Get(id);
            return (desk == null) ? NotFound() : Ok(desk);
        }

        [HttpGet("project")]
        public async Task<IEnumerable<object>> GetProjectDesks(int projectId)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null) 
            {
                return await _deskService.GetProjectDesks(projectId, user.Id).ToListAsync();
            }
            return Array.Empty<CommonModel>();
        }

        [HttpPost]
        public IActionResult Create([FromBody] DeskModel deskModel)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if(user != null) 
            {
                if(deskModel != null)
                {
                    bool res = _deskService.Create(deskModel);
                    return res ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] DeskModel deskModel)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (deskModel != null)
                {
                    bool res = _deskService.Update(id, deskModel);
                    return res ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool res = _deskService.Delete(id);
            return res ? Ok() : NotFound();
        }
    }
}
