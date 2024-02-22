using TaskManager.Common.Models;

namespace TaskManagerApi.Models
{
    public class CommonObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[]? Photo { get; set; }

        #nullable disable
        public CommonObject()
        {
            CreatedDate = DateTime.Now;
        }
        public CommonObject(CommonModel model)
        {
            Name = model.Name;
            Description = model.Description;
            CreatedDate = model.CreatedDate;
            Photo = model.Photo;
        }
    }
}
