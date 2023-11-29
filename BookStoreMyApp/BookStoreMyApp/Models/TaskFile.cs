using System.ComponentModel.DataAnnotations;

namespace BookStoreMyApp.Models
{
    public class TaskFile
    {
       
        [Key]
        public int FileId { get; set; }
        public byte[] Bytes { get; set; }
        public string Description { get; set; }
        public string FileExtension { get; set; }
        public decimal Size { get; set; }


    }
}
