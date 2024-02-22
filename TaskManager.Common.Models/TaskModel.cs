#nullable disable
namespace TaskManager.Common.Models
{
    public class TaskModel : CommonModel
    {
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public byte[] File { get; set; }
        public int DeskId { get; set; }
        public string Column { get; set; }
        public int? CreatorId { get; set; }
        public int? ExecutorId { get; set; }

        public TaskModel()
        {

        }

        public TaskModel(string name, string desc, DateTime start, DateTime end, string column)
        {
            Name = name;
            Description = desc;
            Startdate = start;
            Enddate = end;
            Column = column;
        }
    }
}
