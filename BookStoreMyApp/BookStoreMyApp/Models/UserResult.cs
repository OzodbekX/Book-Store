using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMyApp.Models
{
    public class UserResult
    {
        [Key]
        public int Id { get; set;}
        public int Result { get; set;}  
        public TaskType TaskType { get; set;}


        [ForeignKey("User")]
        public int UserId { get; set;}
        public virtual User User { get; set;}

        [ForeignKey("UserTask")]
        public int UserTaskId { get; set; }
        public virtual UserTask UserTask { get; set; }
    }
}
