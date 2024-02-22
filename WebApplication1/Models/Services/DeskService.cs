using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TaskManager.Common.Models;
using TaskManagerApi.Models.Abstractions;
using TaskManagerApi.Models.Data;

#nullable disable
namespace TaskManagerApi.Models.Services
{
    public class DeskService : AbstractionService, ICommonService<DeskModel>
    {
        private readonly ApplicationContext _db;
        public DeskService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(DeskModel model)
        {
            bool res = DoAction(delegate ()
            { 
                Desk desk = new Desk(model);
                desk.CreatedDate = DateTime.Now;
                _db.Desks.Add(desk);
                _db.SaveChanges();
            });
            return res;
        }

        public bool Delete(int id)
        {
            bool res = DoAction(delegate ()
            {
                Desk desk = _db.Desks.FirstOrDefault(d => d.Id == id);
                _db.Desks.Remove(desk);
                _db.SaveChanges();
            });
            return res;
        }

        public DeskModel Get(int id)
        {
            Desk desk = _db.Desks.Include(d => d.Tasks).FirstOrDefault(d => d.Id == id);
            var deskModel = desk?.ToDto();
            if(deskModel != null)
            {
                deskModel.TasksId = desk.Tasks.Select(t => t.Id).ToList();
            }
            return deskModel;
        }

        public bool Update(int id, DeskModel model)
        {
            bool res = DoAction(delegate ()
            {
                Desk desk = _db.Desks.FirstOrDefault(d => d.Id ==id);
                desk.Name = model.Name;
                desk.Description = model.Description;
                desk.AdminId = model.AdminId;
                desk.Photo = model.Photo;
                desk.IsPrivate = model.IsPrivate;
                desk.Columns = JsonConvert.SerializeObject(model.Columns);

                _db.Desks.Update(desk);
                _db.SaveChanges();
            });
            return res;
        }

        public IQueryable<CommonModel> GetAll(int userId)
        {
            return _db.Desks.Where(d => d.AdminId == userId).Select(d => d.ToShortDto());
        }

        public IQueryable<DeskModel> GetProjectDesks(int projectId, int userId)
        {
            return _db.Desks.Where(d => (d.ProjectId == projectId && d.AdminId == userId || d.IsPrivate == false)).Select(d => d.ToDto());
        }
    }
}
