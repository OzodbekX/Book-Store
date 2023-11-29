namespace BookStoreMyApp.tempModels
{
    public class AuthorViewModel
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? EmailAddress { get; set; }
    }
}
