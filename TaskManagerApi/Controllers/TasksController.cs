using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Models;
using TaskManagerApi.Models.Data;
using TaskManagerApi.Models.Services;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;
        private readonly TaskService _taskService;

        public TasksController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
            _taskService = new TaskService(db);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasksByDesk(int deskId)
        {
            if (_db.Desks.FirstOrDefault(d => d.Id == deskId) != null)
            {
                var res = await _taskService.GetAll(deskId).ToListAsync();
                return Ok(res);
            }
            return NoContent();
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasksForCurrentUser()
        {
            var user = _userService.GetUser(HttpContext.User.Identity?.Name);
            if (user != null)
            {
                var res = await _taskService.GetTaskForUser(user.Id).ToListAsync();
                return Ok(res);
            }
            return Unauthorized(Array.Empty<TaskModel>());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var task = _taskService.Get(id);
            return (task == null) ? NotFound() : Ok(task);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskModel taskModel)
        {
            var user = _userService.GetUser(HttpContext.User.Identity?.Name);
            if (user != null)
            {
                if (taskModel != null)
                {
                    taskModel.CreatorId = user.Id;
                    bool res = _taskService.Create(taskModel);
                    return res ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] TaskModel taskModel)
        {
            var user = _userService.GetUser(HttpContext.User.Identity?.Name);
            if (user != null)
            {
                if (taskModel != null)
                {
                    bool res = _taskService.Update(id, taskModel);
                    return res ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool ret = _taskService.Delete(id);
            return ret ? Ok() : NotFound();
        }
    }
}
