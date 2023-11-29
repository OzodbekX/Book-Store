namespace BookStoreMyApp.ViewModels
{
    public class UserViewModel
    {
        public int? UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        //public string Source { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public short? RoleId { get; set; }
        //[ForeignKey("Pub")]
        //public int PubId { get; set; }
        //public DateTime? HirkeDate { get; set; }
        //public virtual Publisher Pub { get; set; }
    }
}
