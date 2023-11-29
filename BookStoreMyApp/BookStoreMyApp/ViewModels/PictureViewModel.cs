namespace BookStoreMyApp.ViewModels
{
    public class PictureViewModel
    {
        public byte[] Bytes { get; set; }
        public string Description { get; set; }
        public string FileExtension { get; set; }
        public decimal Size { get; set; }
        public int? BookId { get; set; }
    }
}
