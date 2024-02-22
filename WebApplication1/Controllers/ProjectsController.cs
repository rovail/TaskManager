using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Models;
using TaskManagerApi.Models;
using TaskManagerApi.Models.Data;
using TaskManagerApi.Models.Services;

#nullable disable
namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;
        private readonly ProjectService _projectService;

        public ProjectsController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
            _projectService = new ProjectService(db);
        }

        [HttpGet]
        public async Task<IEnumerable<object>> Get()
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if(user.Status == UserStatus.Admin)
                return await _projectService.GetAll().ToListAsync();
            else
            {
                return await _projectService.GetByUserId(user.Id);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var project = _projectService.Get(id);
            return project == null ? NoContent() : Ok(project);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectModel projectModel)
        {
            if (projectModel != null)
            {
                var user = _userService.GetUser(HttpContext.User.Identity.Name);
                if(user != null)
                {
                    if(user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        var admin = _db.ProjectAdmins.FirstOrDefault(a => a.UserId == user.Id);
                        if(admin == null)
                        {
                            admin = new ProjectAdmin(user);
                            _db.ProjectAdmins.Add(admin);
                            _db.SaveChanges();
                        }
                        projectModel.AdminId = admin.Id;
                        projectModel.CreatedDate = DateTime.Now;

                        bool res = _projectService.Create(projectModel);
                        return res ? Ok() : NotFound();
                    }
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id,[FromBody] ProjectModel projectModel)
        {
            if (projectModel != null)
            {
                var user = _userService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        bool res = _projectService.Update(id, projectModel);
                        return res ? Ok() : NotFound();
                    }
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        [HttpPatch("{id}/users")]
        public IActionResult AddUsersToProject(int id, [FromBody] List<int> usersId)
        {
            if(usersId != null)
            {
                var user = _userService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        _projectService.AddUsersToProject(id, usersId);
                        return Ok();
                    }
                    return Unauthorized();
                }
            }   
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool res = _projectService.Delete(id);
            return res ? Ok() : NotFound();
        }

        [HttpPatch("{id}/users/remove")]
        public IActionResult RemoveUsersFromProject(int id, [FromBody] List<int> usersId)
        {
            if (usersId != null)
            {
                var user = _userService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        _projectService.RemoveUsersFromProject(id, usersId);
                        return Ok();
                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }
    }
}
