using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TaskManager.Common.Models;
using TaskManagerApi.Migrations;
using TaskManagerApi.Models.Abstractions;
using TaskManagerApi.Models.Data;

namespace TaskManagerApi.Models.Services
{ 
    public class TaskService : AbstractionService, ICommonService<TaskModel>
    {
        private readonly ApplicationContext _db;
        public TaskService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(TaskModel model)
        {
            bool res = DoAction(delegate ()
            {
                Task task = new Task(model);
                task.CreatedDate = DateTime.Now;
                _db.Tasks.Add(task);
                _db.SaveChanges();
            });
            return res;
        }

        public bool Delete(int id)
        {
            bool res = DoAction(delegate ()
            {
                Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
                if(task != null)
                    _db.Tasks.Remove(task);
                _db.SaveChanges();
            });
            return res;
        }

        public TaskModel Get(int id)
        {
            Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            #pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
            return task?.ToDto();
            #pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
        }

        public bool Update(int id, TaskModel model)
        {
            bool res = DoAction(delegate ()
            {
                Task task = _db.Tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    task.Name = model.Name;
                    task.Description = model.Description;
                    task.CreatedDate = model.CreatedDate;
                    task.Startdate = model.Startdate;
                    task.Photo = model.Photo;
                    task.Startdate = model.CreatedDate;
                    task.Enddate = model.Enddate;
                    task.File = model.File;
                    task.Column = model.Column;
                    task.ExecutorId = model.ExecutorId;

                    _db.Tasks.Update(task);
                    _db.SaveChanges();
                }
            });
            return res;
        }

        public IQueryable<TaskModel> GetAll(int deskId) 
        {
            return _db.Tasks.Where(t => t.DeskId == deskId).Select(t => t.ToShortDto());
        }

        public IQueryable<TaskModel> GetTaskForUser(int userId)
        {
            return _db.Tasks.Where(t => t.CreatorId == userId || t.ExecutorId == userId).Select(t => t.ToShortDto());
        }
    }
}
