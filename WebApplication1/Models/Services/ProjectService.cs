using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Models;
using TaskManagerApi.Models.Abstractions;
using TaskManagerApi.Models.Data;

#nullable disable
namespace TaskManagerApi.Models.Services
{
    public class ProjectService : AbstractionService, ICommonService<ProjectModel>
    {
        private readonly ApplicationContext _db;

        public ProjectService(ApplicationContext db)
        {
            _db = db;
        }
        public bool Create(ProjectModel model)
        {
            bool res = DoAction(delegate ()
            {
                Project project = new Project(model);
                project.CreatedDate = DateTime.Now;
                _db.Projects.Add(project);
                _db.SaveChanges();
            });
            return res;
        }

        public bool Delete(int id)
        {
            bool res = DoAction(delegate ()
            {
                Project project = _db.Projects.FirstOrDefault(x => x.Id == id);
                _db.Projects.Remove(project);
                _db.SaveChanges();
            });
            return res;
        }

        public bool Update(int id, ProjectModel model)
        {
            bool res = DoAction(delegate ()
            { 
                Project project = _db.Projects.FirstOrDefault(p => p.Id == id);
                project.Name = model.Name;
                project.Description = model.Description;
                project.Photo = model.Photo;
                project.Status = model.Status;
                _db.Projects.Update(project);
                _db.SaveChanges();
            });
            return res;
        }

        public ProjectModel Get(int id)
        {
            Project project = _db.Projects.Include(p => p.AllUsers).Include(p => p.AllDesks).FirstOrDefault(p  => p.Id == id);

            var projectModel = project?.ToDto();
            if (projectModel != null)
            {
                projectModel.AllUsers = project.AllUsers.Select(u => u.Id).ToList();
                projectModel.AllDesks = project.AllDesks.Select(d => d.Id).ToList();
            }

            return projectModel;
        }

        public async Task<IEnumerable<ProjectModel>> GetByUserId(int userId)
        {
            List<ProjectModel> res = new List<ProjectModel>();
            var admin = _db.ProjectAdmins.FirstOrDefault(a => a.UserId == userId);
            if (admin != null)
            {
                var projectsForAdmin = await _db.Projects.Where(p => p.AdminId == admin.Id).Select(p => p.ToDto()).ToListAsync();
                res.AddRange(projectsForAdmin);
            }
            var projectsForUser = await _db.Projects.Include(p => p.AllUsers).Where(p => p.AllUsers.Any(u => u.Id == userId)).Select(p => p.ToDto()).ToListAsync();
            res.AddRange(projectsForUser);
            return res;
        }
        public IQueryable<CommonModel> GetAll()
        {
            return  _db.Projects.Select(p => p.ToDto() as CommonModel);
        }
        public void AddUsersToProject(int id, List<int> usersId)
        {
            Project project = _db.Projects.Include(p => p.AllUsers).FirstOrDefault(p => p.Id == id);

            foreach (int userId in usersId)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null && !project.AllUsers.Any(u => u.Id == userId))
                {
                    project.AllUsers.Add(user);
                }
            }

            _db.SaveChanges();
        }
        public void RemoveUsersFromProject(int id, List<int> usersId)
        {
            Project project = _db.Projects.Include(p => p.AllUsers).FirstOrDefault(p => p.Id == id);

            foreach (int userId in usersId)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                if(project.AllUsers.Contains(user))
                    project.AllUsers.Remove(user);
            }
            _db.SaveChanges();
        }
    }
}
