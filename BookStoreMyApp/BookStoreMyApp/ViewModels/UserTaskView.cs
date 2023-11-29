namespace BookStoreMyApp.ViewModels
{
    public class UserTaskView
    {
        public int UserTaskId { get; set; }
        public string UserTaskTittle { get; set; }
        public string TaskAddress { get; set; }
        public string Description { get; set; }
        public IFormFile TaskFile { get; set; }
    }
}
