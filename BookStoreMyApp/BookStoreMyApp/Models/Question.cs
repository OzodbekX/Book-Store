using System.ComponentModel.DataAnnotations;

namespace BookStoreMyApp.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string TaskAddress { get; set; }
        public string QuestionText { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Answer { get; set; }
        public int? Quizball { get; set; }

    }
}
