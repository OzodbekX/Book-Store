using BookStoreMyApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BookStoreMyApp.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
        public int? PubId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Advance { get; set; }
        public byte? AuthorOrder { get; set; }
        public int? RoyaltyPercentage { get; set; } 
        public int? Royalty { get; set; }
        public int? YtdSales { get; set; }
        public virtual ICollection<BookAuthor>? BookAuthors { get; set; }
        public virtual ICollection<Picture>? Pictures { get; set; }

        public string? Notes { get; set; }
        public DateTime PublishedDate { get; set; }
        public IFormFileCollection Files { get; set; }

    }
}
