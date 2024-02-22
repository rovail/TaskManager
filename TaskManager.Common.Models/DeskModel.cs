#nullable disable
namespace TaskManager.Common.Models
{
    public class DeskModel : CommonModel
    {
        public bool IsPrivate { get; set; }
        public string[] Columns { get; set; }
        public int AdminId { get; set; }
        public int ProjectId { get; set; }
        public List<int> TasksId { get; set; }

        public DeskModel()
        {
            
        }

        public DeskModel(string name, string desc, bool isprivate, string[] columns)
        {
            Name = name;
            Description = desc;
            IsPrivate = isprivate;
            Columns = columns;
        }
    }
}
