using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreMyApp.Models
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        public byte[] Bytes { get; set; }
        public string Description { get; set; }
        public string FileExtension { get; set; }
        public decimal Size { get; set; }
        [ForeignKey("Book")]
        public int? BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
