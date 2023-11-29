using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMyApp.Models
{
    public class UserTask
    {
        public UserTask()
        {
            TaskFile = new HashSet<TaskFile>();
        }
        [Key]
        public int UserTaskId { get; set; }
        public string UserTaskTittle { get; set; }
        public string TaskAddress { get; set; }
        public string Description { get; set; }
        public virtual ICollection<TaskFile> TaskFile { get; set; } = null!;


    }
}
