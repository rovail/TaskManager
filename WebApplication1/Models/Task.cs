#nullable disable

using Newtonsoft.Json;
using TaskManager.Common.Models;

namespace TaskManagerApi.Models
{
    public class Task : CommonObject
    {
        public int Id { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public byte[] File {  get; set; }
        public int? DeskId { get; set; }
        public Desk Desk { get; set; }
        public string Column {  get; set; }
        public int? CreatorId { get; set; }
        public User Creator { get; set; }
        public int? ExecutorId { get; set; }

        public Task() {}
        public TaskModel ToDto()
        {
            return new TaskModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreatedDate = this.CreatedDate,
                Photo = this.Photo,
                Startdate = this.CreatedDate,
                Enddate = this.Enddate,
                File = this.File,
                DeskId = this.DeskId.HasValue ? this.DeskId.Value : 0,
                Column = this.Column,
                CreatorId = this.CreatorId.HasValue ? this.CreatorId.Value : 0,
                ExecutorId = this.ExecutorId.HasValue ? this.ExecutorId.Value : 0
            };
        }
        public TaskModel ToShortDto()
        {
            return new TaskModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreatedDate = this.CreatedDate,
                Startdate = this.CreatedDate,
                Enddate = this.Enddate,
                DeskId = this.DeskId.HasValue ? this.DeskId.Value : 0,
                Column = this.Column,
                CreatorId = this.CreatorId.HasValue ? this.CreatorId.Value : 0,
                ExecutorId = this.ExecutorId.HasValue ? this.ExecutorId.Value : 0
            };
        }

        public Task(TaskModel taskModel) : base(taskModel)
        {
            Id = taskModel.Id;
            Startdate = taskModel.CreatedDate;
            Enddate = taskModel.Enddate;
            File = taskModel.File;
            DeskId = taskModel.DeskId;
            Column = taskModel.Column;
            CreatorId = taskModel.CreatorId;
            ExecutorId = taskModel.ExecutorId;
        }
    }
}
