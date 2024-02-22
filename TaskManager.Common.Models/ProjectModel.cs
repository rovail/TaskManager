#nullable disable
namespace TaskManager.Common.Models
{
    public class ProjectModel : CommonModel
    {
        public int? AdminId { get; set; }
        public ProjectStatus Status { get; set; }
        public List<int> AllUsers { get; set; }
        public List<int> AllDesks { get; set; }

        public ProjectModel(){ }
        public ProjectModel(string name,  string desc, ProjectStatus status, int adminId)
        {
            Name = name;
            Description = desc;
            Status = status;
            AdminId = adminId;
        }
    }
}
