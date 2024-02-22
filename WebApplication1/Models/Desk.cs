﻿#nullable disable

using TaskManager.Common.Models;
using Newtonsoft.Json;

namespace TaskManagerApi.Models 
{
    public class Desk : CommonObject
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public string Columns { get; set; }
        public int AdminId { get; set; }
        public User Admin { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
        public DeskModel ToDto()
        {
            return new DeskModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreatedDate = this.CreatedDate,
                Photo = this.Photo,
                AdminId = this.AdminId,
                IsPrivate = this.IsPrivate,
                Columns = JsonConvert.DeserializeObject<string[]>(this.Columns),
                ProjectId = this.ProjectId
            };
        }
        public CommonModel ToShortDto()
        {
            return new CommonModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreatedDate = this.CreatedDate,
                Photo = this.Photo,
            };
        }
        public Desk(DeskModel deskModel) : base(deskModel)
        {
            Id = deskModel.Id;
            AdminId = deskModel.AdminId;
            IsPrivate = deskModel.IsPrivate;
            ProjectId = deskModel.ProjectId;

            if(deskModel.Columns.Any())
                Columns = JsonConvert.SerializeObject(deskModel.Columns);
        }
        public Desk()
        {
            
        }
    }
}
