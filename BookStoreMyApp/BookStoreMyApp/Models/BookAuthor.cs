using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMyApp.Models
{
    public partial class BookAuthor
    {
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public byte? AuthorOrder { get; set; }
        public int? RoyaltyPercentage { get; set; }
        public virtual Author Author { get; set; } = null!;
        public virtual Book Book { get; set; } = null!;
    }
}
